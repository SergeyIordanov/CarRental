// image scaling
$(document).ready(function () {
    $('.materialboxed').materialbox();
});

// dropdown select
$(document).ready(function () {
    $('select').material_select();
});

// datepickers
$('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    selectYears: 10 // Creates a dropdown of 15 years to control year
});

// range using noUiSlider
var range = document.getElementById('range');

noUiSlider.create(range, {
    start: [0, 3000], // Handle start position
    step: 1, // Slider moves in increments of '1'
    margin: 0, // Handles must be more than '0' apart
    connect: true, // Display a colored bar between the handles
    direction: 'ltr', // Put '0' at the left side of the slider
    orientation: 'horizontal', // Orient the slider horizontally
    behaviour: 'tap-drag', // Move handle on tap, bar is draggable
    range: { // Slider can select '0' to '3000'
        'min': 0,
        'max': 3000
    }
    //pips: { // Show a scale with the slider
    //    mode: 'steps',
    //    density: 100
    //}
});


var valueInputLow = document.getElementById('value-input-low'),
    valueInputHigh = document.getElementById('value-input-high');

// When the slider value changes, update the input and span
range.noUiSlider.on('update', function (values, handle) {
    if (handle) {
        valueInputHigh.value = values[handle];
    } else {
        valueInputLow.value = values[handle];
    }
});

// When the input changes, set the slider value
valueInputHigh.addEventListener('change', function () {
    range.noUiSlider.set([null, this.value]);
});

valueInputLow.addEventListener('change', function () {
    range.noUiSlider.set([this.value, null]);
});