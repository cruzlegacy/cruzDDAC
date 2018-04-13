


$(document).ready(function () {

    // Example 2. Callback functions
    var example_slider = $('.example2').fws2();

    example_slider.settings({

        beforeInit: function () {
            // alert("Slider preparing!");
        },

        afterInit: function () {
            alert('Slider ready!');
        },

        slideStart: function (slide) {
            // alert("Slide Start!");
        },

        slideEnd: function (slide) {

            var title = slide.find(".title").text();

            alert('This is ' + title);
        }
    });

    example_slider.addSlide({ image: 'img/slide_1.jpg', title: 'Slide 1', description: 'Description', link: 'http://website' });
    example_slider.addSlide({ image: 'img/slide_2.jpg', title: 'Slide 2', description: 'Description', link: 'http://website' });
    example_slider.addSlide({ image: 'img/slide_3.jpg', title: 'Slide 3', description: 'Description', link: 'http://website' });

    example_slider.start();

});
