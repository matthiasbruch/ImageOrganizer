var commandField = function ($j) {

    var isOpened = false;
    var isAnimating = false;
    var inputBoxContainer = null;
    var commandBox = null;
    var commandSuggestList = null;
    var folderCommands = ['move-file', 'move-folder', 'tag', 'select'];

    var handleEnterPress = function (keyCode) {
        if (!isAnimating) {
            var newStateOpened = !isOpened;
            isAnimating = true;

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

        commandSuggestList.html('');

        if (commandText) {
            for (var commandIdx = 0; commandIdx < folderCommands.length; commandIdx++)
            {
                var loopedCommand = folderCommands[commandIdx];
                if (loopedCommand.indexOf(commandText) === 0)
                {
                    commandSuggestList.append('<div>- ' + loopedCommand + '</div>');
                }
            }

            e.preventDefault();
            e.stopPropagation();
        }
    };

    return {
        Init: function () {
            inputBoxContainer = $j('#input-box-container');
            commandBox = inputBoxContainer.find('.command-box');
            commandSuggestList = inputBoxContainer.find('.command-suggest-list');

            commandBox.on('keyup', handleCommandBoxInput);

            $j(document).on('keyup', "body", function (e) {
                var keyCode = e.charCode;
                if (!keyCode)
                {
                    keyCode = e.keyCode;
                }

                if (keyCode === 13)
                {
                    handleEnterPress();

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