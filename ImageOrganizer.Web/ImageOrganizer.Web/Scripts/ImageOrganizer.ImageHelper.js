var imageHelper = function ($j) {

    var handleMouseDown = function (e) {
        if (e.which === 3) {
            
            // TODO: Handle right click.

            e.preventDefault();
            e.stopPropagation();
        }
    }
    
    var handleHeaderBarClicked = function (e) {
        var clickedHeader = $j(this);
        var cbUseThis = clickedHeader.find('[name=cbUseThis]');

        if (cbUseThis.prop('checked')) {
            cbUseThis.prop('checked', false);
            clickedHeader.removeClass('selected');
        }
        else {
            cbUseThis.prop('checked', true);
            clickedHeader.addClass('selected');
        }

        e.preventDefault();
        e.stopPropagation();
    }

    return {
        init: function () {
            $j('.previewImage').on('mousedown', handleMouseDown);

            $j('.handleHeaderBar').on('click', handleHeaderBarClicked);
        }
    };
}(jQuery);

jQuery(function () {
    imageHelper.init();
});