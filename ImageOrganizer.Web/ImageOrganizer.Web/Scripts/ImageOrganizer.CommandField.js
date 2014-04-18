var commandField = function ($j) {

    var commandConfig = {};

    var isOpened = false;
    var isAnimating = false;
    var inputBoxContainer = null;
    var commandBox = null;
    var commandSuggestList = null;
    var commandOutputContainer = null;
    var commandTextOutput = null;
    var folderCommands = ['move-file', 'move-folder', 'tag', 'select'];
    folderCommands.sort();

    var KEY_UP = 38;
    var KEY_DOWN = 40;
    var KEY_ENTER = 13;
    var KEY_ESC = 27;

    var updateSuggestFromServer = function () {
        if (commandConfig && commandConfig.CommandName)
        {
            $j.ajax({
                url: '/Suggest/GetItems',
                data: commandConfig,
                traditional:true
            }).done(function (result) {
                if (result) {
                    for (var resultIdx = 0; resultIdx < result.length; resultIdx++) {
                        var resultItem = result[resultIdx];
                        createSuggestListItem(resultItem.Label, resultItem.Value, resultItem.ListItemType, true);
                    }
                }
            });
        }
    };

    var executeOnServer = function () {
        if (commandConfig && commandConfig.CommandName) {
            $j.ajax({
                url: '/Command/Execute',
                data: commandConfig,
                traditional: true
            }).done(function (result) {
                window.location.href = window.location.href;
            });
        }
    };

    var resetCommandConfig = function () {
        commandConfig.CommandName = "";
        commandConfig.Parameter = "";
        commandConfig.CurrentPath = $j('.current-path').val();
        commandConfig.Ids = [];
        
        var selectedItemsOnPage = $j(".checkbox-use-this").filter(":checked");
        selectedItemsOnPage.each(function () {
            commandConfig.Ids.push($j(this).val());
        });

        if (commandTextOutput) {
            commandTextOutput.text("");
        }
    };

    var createSuggestListItem = function (label, param, listItemType, isFromSuggest) {
        var newElement = $j('<div class="suggest-list-item" data-param="' + param + '">' + label + '</div>');

        if (listItemType)
        {
            newElement.addClass('list-type-' + listItemType);
        }

        if (isFromSuggest)
        {
            newElement.addClass('use-as-param');
        }

        commandSuggestList.append(newElement);
    };

    var toggleCommandLayer = function () {
        if (!isAnimating) {
            var newStateOpened = !isOpened;
            isAnimating = true;
            resetCommandConfig();

            if (newStateOpened) {
                inputBoxContainer.stop().slideDown('fast', function () {
                    inputBoxContainer.find('input').focus();

                    isAnimating = false;
                    isOpened = true;
                });
            }
            else {
                inputBoxContainer.stop().slideUp('fast', function () {
                    isAnimating = false;
                    isOpened = false;
                });
            }
        }
    };

    var handleCommandBoxInput = function (e) {
        var commandText = commandBox.val();

        // Getting keycode.
        // [MB]
        var keyCode = null;
        if (e) {
            keyCode = e.charCode;
            if (!keyCode) {
                keyCode = e.keyCode;
            }
        }

        if ((keyCode == KEY_UP) || (keyCode == KEY_DOWN))
        {
            handleSelectSuggestItem(keyCode);
        }
        else if (keyCode == KEY_ENTER)
        {
            var selectedItem = commandSuggestList.find('.suggest-list-item.selected');
            if (selectedItem.length > 0)
            {
                if (selectedItem.hasClass('list-type-execute'))
                {
                    executeOnServer();
                }
                else {
                    confirmSuggestedItem(selectedItem);
                    updateSuggestFromServer();
                }
            }
        }
        else if (keyCode == KEY_ESC)
        {
            toggleCommandLayer();
            resetCommandConfig();
        }
        else {
            commandSuggestList.html('');

            for (var commandIdx = 0; commandIdx < folderCommands.length; commandIdx++) {
                var loopedCommand = folderCommands[commandIdx];
                if ((commandText === "") || (loopedCommand.indexOf(commandText) === 0)) {
                    createSuggestListItem(loopedCommand);
                }
            }
        }

        if (e) {
            e.preventDefault();
            e.stopPropagation();
        }
    };

    var confirmSuggestedItem = function (suggestedItem) {
        if (suggestedItem && (suggestedItem.length > 0)) {
            var useAsParam = suggestedItem.hasClass('use-as-param');
            var selectedText = suggestedItem.text();

            suggestedItem.removeClass('selected');
            commandBox.val('');
            
            if (useAsParam) {
                commandConfig.Parameter += selectedText + '\\';
            }
            else {
                commandConfig.CommandName = selectedText;
                commandConfig.Parameter = "";
            }

            commandTextOutput.text(commandConfig.CommandName + ' ' + commandConfig.Parameter);
            commandSuggestList.html('');
        }
    };

    var handleSelectSuggestItem = function (keyCode) {
        var allItemsInList = commandSuggestList.find('.suggest-list-item');

        var currentlySelectedElement = allItemsInList.filter('.selected');

        if (currentlySelectedElement.length === 0) {
            if (keyCode == KEY_DOWN) {
                allItemsInList.first().addClass('selected');
            }
            else if (keyCode == KEY_UP) {
                allItemsInList.last().addClass('selected');
            }
        }
        else {
            var nextItem = null;
            if (keyCode == KEY_DOWN) {
                nextItem = currentlySelectedElement.next();
            }
            else if (keyCode == KEY_UP) {
                nextItem = currentlySelectedElement.prev();
            }

            if (nextItem && nextItem.length > 0) {
                currentlySelectedElement.removeClass('selected');
                nextItem.addClass('selected');
            }
        }
    }

    return {
        Init: function () {
            resetCommandConfig();

            inputBoxContainer = $j('#input-box-container');
            commandBox = inputBoxContainer.find('.command-box');
            commandSuggestList = inputBoxContainer.find('.command-suggest-list');
            commandOutputContainer = inputBoxContainer.find('.current-command');
            commandTextOutput = commandOutputContainer.find('.command-text');

            commandBox.on('keyup', handleCommandBoxInput);

            $j(document).on('keyup', "body", function (e) {
                var keyCode = e.charCode;
                if (!keyCode)
                {
                    keyCode = e.keyCode;
                }

                if (keyCode === KEY_ENTER)
                {
                    toggleCommandLayer();
                    handleCommandBoxInput();

                    e.preventDefault();
                    e.stopPropagation();
                }
            });
        }
    };
}(jQuery);

jQuery(function () {
    commandField.Init();
});