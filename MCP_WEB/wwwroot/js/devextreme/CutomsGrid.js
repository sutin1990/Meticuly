class CustomGrid {
    constructor(gridItems) {
        gridItems.clearSelection();
    }
}

var NotSelection = function (gridItems) {

    gridItems.clearSelection();
    console.log(gridItems);
};