function MilesImageCrop_CropImage(panel, aspectRatio, trueSize) {
    // Create variables (in this scope) to hold the API and image size
    var jcrop_api,
    boundx,
    boundy,

    // Grab some information about the preview pane
    $preview = $('#' + panel + ' .preview-pane'),
    $pcnt = $('#' + panel + ' .preview-pane .preview-container'),
    $pimg = $('#' + panel + ' .preview-pane .preview-container img'),
    $croppane = $('#' + panel + ' .image-pane');
    $cropimg = $('#' + panel + ' .image-pane > img');

    $pcnt.height($pcnt.width() / aspectRatio - 14);
    $cropimg.width($croppane.width() );


    xsize = $pcnt.width(),
    ysize = $pcnt.height();

    console.log('init', [xsize, ysize]);

    //restore selection from where it was (in case of a postback)
    //var rawstate = $('#' + panel + ' input[id$="ClientState"]').val();
    //var state = { cX: 0, cY: 0, cW: 200, cH: 0 }
    //if (rawstate.length) {
    //    var state = $.parseJSON(rawstate);
    //}

    //if (parseInt(state.cW) == 0) state.cW = 200;
    //if (parseInt(state.cH) == 0) state.cH = 200;

    $cropimg.Jcrop({
        trueSize: trueSize,
        onChange: MilesImageCrop_UpdatePreview,
        onSelect: MilesImageCrop_UpdatePreview,
        aspectRatio: aspectRatio,
        setSelect: [0,0,200,200]
    }, function () {
        // Use the API to get the real image size
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        // Store the API in the jcrop_api variable
        jcrop_api = this;

        // Move the preview into the jcrop container for css positioning
        //$preview.appendTo(jcrop_api.ui.holder);
    });

    function MilesImageCrop_UpdatePreview(c) {
        if (parseInt(c.w) > 0) {
            //store the coordinates and image dimensions
            $('#' + panel + ' input[id$="ClientState"]').val(JSON.stringify(
                {
                    cX: c.x,
                    cY: c.y,
                    cW: c.w,
                    cH: c.h
                }));

            
            console.log('c', [c.x, c.y, c.w, c.h]);

            var rx = xsize / c.w;
            var ry = ysize / c.h;
            console.log('crop', [c.w, c.h]);
            $pimg.css({
                width: Math.round(rx * boundx) + 'px',
                height: Math.round(ry * boundy) + 'px',
                marginLeft: '-' + Math.round(rx * c.x) + 'px',
                marginTop: '-' + Math.round(ry * c.y) + 'px'
            });
        }
    }
}