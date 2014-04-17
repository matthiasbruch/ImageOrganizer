var imageHelper = function ($j) {

    var handleMouseDown = function (e) {
        if (e.which === 3) {
            
            // TODO: Handle right click.

            e.preventDefault();
            e.stopPropagation();
        }
    }

    return {
        init: function () {
            $j('.previewImage').on('mousedown', handleMouseDown);
        }
    };
}(jQuery);

jQuery(function () {
    imageHelper.init();
});