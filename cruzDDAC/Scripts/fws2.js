(function ($) {

    "use strict";
    
    // from jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
    $.extend($.easing,
    {
        FWS2easeOutCubic: function (x, t, b, c, d) {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        },
        FWS2easeInOutExpo: function (x, t, b, c, d) {
            if (t == 0)
                return b;
            if (t == d)
                return b + c;
            if ((t /= d / 2) < 1)
                return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
            return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
        }
    });

    $.fn.fws2 = function (options) {
        
        var glob = {
            options: {
                cs              : 0,
                duration        : 750,
                hoverpause      : 1,
                pause           : 6000,
                arrows          : 1,
                bullets         : 1,
                height          : 600,
                expandDuration  : 750,
                linktext        : "Join Us for More",
                
                titleHTML       : '<h4>%title%</h4>',             
                descriptionHTML : '<p>%desc%</p>',                     
                linkHTML        : '<a href="%link%">%linktext%</a>',   
               
                beforeInit      : function() {},
                afterInit       : function() {},
                slideStart      : function() {},
                slideEnd        : function() {}
            },
            slides: []
        };

        $.extend(glob.options, options);

        var randID = "fws2-instance-" + new Date().getTime();

        $('<div id="' + randID + '" class="fws2"></div>').appendTo(this);

        var $instance = $("#" + randID);

        var content = {
            
            resize: function () {
                $instance.css({height: $instance.find(".slide:first").height()});
            },
            
            replaceHTML: function(html, th){
                
                th.linktext = th.linktext ? th.linktext : glob.options.linktext;
                
                return html.replace("%title%", th.title)
                        .replace("%desc%", th.description)
                        .replace("%link%", th.link)
                        .replace("%linktext%", th.linktext);
                
            },
            
            buildHTML: function () {
                
                var parent = this;
                
                var html = "";
                html += '<div class="slider_container">';
                    $(glob.slides).each(function () {

                        var linktext = this.linktext ? this.linktext : glob.options.linktext;
                        
                        html += '<div class="slide">';
                            html += '<img src="' + this.image + '" alt="'+this.alt+'">';
                            html += '<div class="slide_content">';
                                html += '<div class="slide_content_wrap">';
                                    html += '<div class="title">' + parent.replaceHTML(glob.options.titleHTML, this) + '</div>';
                                    html += '<div class="description">' + parent.replaceHTML(glob.options.descriptionHTML, this) + '</div>';
                                    html += '<div class="readmore">' + parent.replaceHTML(glob.options.linkHTML, this) + '</div>';
                                html += '</div>';
                            html += '</div>';
                        html += '</div>';
                    });
                html += '</div>';

                if (glob.options.arrows == "1") {
                    html += '<div class="slidePrev"><i class="fa fa-chevron-left"></i></div>';
                    html += '<div class="slideNext"><i class="fa fa-chevron-right"></i></div>';
                }
                
                html += '<div class="timers"></div>';

                if (glob.options.bullets == "1") {
                    html += '<div class="bullets"></div>';
                }

                $(html).appendTo($instance);

                for (var i = 0; i < glob.slides.length; i++) {
                    $('<div class="timer"><div class="progress"></div></div>').appendTo($instance.find(".timers"));
                    $('<div class="bullet"><i class="fa fa-circle"></i></div>').appendTo($instance.find(".bullets"));
                }
            },
            
            imgLoaded: function ($img, callback, step) {
                
                if (typeof $.fn.imagesLoaded !== "undefined") {
                    // imagesLoaded script is loaded
                    $img.imagesLoaded(function () {
                        if (typeof callback === "function") {
                            callback();
                        }
                    });
                    return;
                }
            
                var total = $img.length;
                var loaded = 0;

                if (total <= 0) {
                    if (typeof callback === "function") {
                        callback();
                    }
                }

                $img.each(function () {
                    var image = new Image;
                    image.onload = function () {

                        if (typeof step === "function") {
                            step();
                        }

                        loaded++;
                        if (loaded >= total) {
                            if (typeof callback === "function") {
                                callback();
                            }
                        }
                    };

                    image.onerror = function () {
                        console.warn("Could not load image");
                    };

                    image.src = $(this).attr("src");
                });
            },
            
            init: function () {

                /* Build html */
                this.buildHTML();
                
                this.imgLoaded($instance.find("img"), function () {
                   
                    $instance.find(".bullets .bullet").eq(glob.options.cs).addClass("active");
                    $instance.find(".timers").css({
//                        width: ($instance.find(".timers .timer").length + 1) * $instance.find(".timers .timer").width() + 5
                    });
                    $instance.find(".slide").eq(glob.options.cs).fadeIn({
                        duration: glob.options.expandDuration,
                        easing: "swing"
                    });
                    $instance.animate({
                        height: $instance.find(".slide:first").height()
                    }, {
                        duration: glob.options.expandDuration,
                        easing: "FWS2easeInOutExpo",
                        complete: function () {

                            // Arrows and bullets animation show duration

                            var controls_anim_duration = 1500;

                            $instance.find(".slidePrev").animate({
                                left: 0
                            }, {
                                duration: controls_anim_duration,
                                easing: "FWS2easeInOutExpo"
                            });

                            $instance.find(".slideNext").animate({
                                right: 0
                            }, {
                                duration: controls_anim_duration,
                                easing: "FWS2easeInOutExpo"
                            });

                            $instance.find(".bullets").animate({
                                bottom: 15
                            }, {
                                duration: controls_anim_duration,
                                easing: "FWS2easeInOutExpo"
                            });

                            content.showText();
                            controls.position();
                            content.resize();
                            auto.run();
                            auto.focus();

                            setTimeout(function () {
                                if (typeof glob.options.afterInit === "function") {
                                    glob.options.afterInit();
                                }
                            }, controls_anim_duration);

                        }
                    });

                });
            },
            
            show: function () {
                /* Show slide */

                content.hideText();

                $instance.find(".bullets .bullet").removeClass("active");
                $instance.find(".bullets .bullet").eq(glob.options.cs).addClass("active");

                $instance.find(".slide").eq(glob.options.cs).css({
                    opacity: 0,
                    zIndex: 2
                }).show().animate({
                    opacity: 1
                }, {
                    queue: false,
                    duration: glob.options.duration,
                    easing: "swing",
                    start: function () {
                        if (typeof glob.options.slideStart === "function") {
                            glob.options.slideStart($instance.find(".slide").eq(glob.options.cs));
                        }
                    },
                    complete: function () {
                        $instance.find(".slide").filter(":lt(" + glob.options.cs + ")").css("zIndex", 0).hide();
                        $instance.find(".slide").filter(":gt(" + glob.options.cs + ")").css("zIndex", 0).hide();
                        $instance.find(".slide").eq(glob.options.cs).css("zIndex", 1);

                        content.showText();
                        auto.run();

                        if (typeof glob.options.slideEnd === "function") {
                            glob.options.slideEnd($instance.find(".slide").eq(glob.options.cs));
                        }
                    }
                });
            },
            
            showText: function () {

                var $slide = $instance.find(".slide").eq(glob.options.cs);

                /* Position Text */
                var textWrapper = $slide.find(".slide_content_wrap");

                textWrapper.css({
                    top: "50%",
                    left: 0,
                    display: "block"
                });

                textWrapper.css({
                    marginTop: -1 * textWrapper.height() / 2,
                    marginLeft: "8%"
                });

                /* Show slide text */
                $slide.find(".title").fadeTo(300, 1);

                setTimeout(function () {
                    $slide.find(".description").fadeTo(300, 1);
                }, 150);

                setTimeout(function () {
                    $slide.find(".readmore").fadeTo(300, 1);
                }, 300);
            },
            
            hideText: function () {
                /* Hide slide text */

                $instance.find(".slide .title").fadeTo(300, 0);

                setTimeout(function () {
                    $instance.find(".slide .description").fadeTo(300, 0);
                }, 150);

                setTimeout(function () {
                    $instance.find(".slide .readmore").fadeTo(300, 0);
                }, 300);
            },
            
            slidePrev: function() {
                
                if ($instance.find(".slide").is(":animated")) {
                    return;
                }

                if (glob.options.cs <= 0) {
                    glob.options.cs = $instance.find(".slide:last").index();

                    $instance.find(".timers .timer .progress").stop();
                    $instance.find(".timers .timer .progress").css({
                        width: "100%"
                    });
                    $instance.find(".timers .timer:last .progress").animate({
                        width: "0%"
                    }, {
                        queue: false,
                        duration: glob.options.duration,
                        easing: "FWS2easeOutCubic"
                    });

                } else {
                    glob.options.cs--;

                    $instance.find(".timers .timer .progress").stop();
                    $instance.find(".timers .timer:gt(" + glob.options.cs + ") .progress").css({
                        width: "0%"
                    });
                    $instance.find(".timers .timer:eq(" + glob.options.cs + ") .progress").animate({
                        width: "0%"
                    }, {
                        queue: false,
                        duration: glob.options.duration,
                        easing: "FWS2easeOutCubic"
                    });
                }

                this.show();
            },
            
            slideNext: function(){

                if ($instance.find(".slide").is(":animated")) {
                    return;
                }

                if ($instance.find(".slide").eq(glob.options.cs + 1).length <= 0) {

                    glob.options.cs = 0;

                    $instance.find(".timers .timer .progress").stop();

                    $instance.find(".timers .timer:last .progress").animate({
                        width: "100%"
                    }, {
                        queue: false,
                        duration: glob.options.duration,
                        easing: "FWS2easeOutCubic",
                        complete: function () {
                            $instance.find(".timers .timer .progress").css({
                                width: "0%"
                            });
                        }
                    });
                } else {
                    glob.options.cs++;

                    $instance.find(".timers .timer .progress").stop();
                    $instance.find(".timers .timer:lt(" + glob.options.cs + ") .progress").animate({
                        width: "100%"
                    }, {
                        queue: false,
                        duration: glob.options.duration,
                        easing: "FWS2easeOutCubic"
                    });

                }
                
                this.show();
            }
        };

        var controls = {
            
            /* Adjust buttons position */
            position: function () {

                $instance.find(".slidePrev, .slideNext").css({
                    top: $instance.height() / 2 - $instance.find(".slideNext").height() / 2
                });

                $instance.find(".slidePrev").css({left: 0});
                $instance.find(".slideNext").css({right: 0});
            },
            
            bindControls: function () {
                
                if ($.mobile) {
                    $instance.on("swiperight", function (e) {
                        content.slidePrev();
                    });

                    $instance.on("swipeleft", function (e) {
                        content.slideNext();
                    });
                }
                
                $(window).on("resize",function () {
                    content.resize();
                    controls.position();
                });

                /* Hover effect */
                $instance.find(".slidePrev, .slideNext").on("mouseover", function () {
                    $(this).animate({
                        opacity: 1
                    }, {
                        queue: false,
                        duration: 1000,
                        easing: "FWS2easeOutCubic"
                    });
                });

                /* Hover effect - mouseout */
                $instance.find(".slidePrev, .slideNext").on("mouseout", function () {
                    $(this).animate({
                        opacity: 0.5
                    }, {
                        queue: false,
                        duration: 1000,
                        easing: "FWS2easeOutCubic"
                    });
                });

                /* Arrow Key Bind */
                $(document).on("keyup", function (e) {
                    if (e.which === 39) {
                        $instance.find(".slideNext").trigger("click");
                    }
                    if (e.which === 37) {
                        $instance.find(".slidePrev").trigger("click");
                    }
                });

                /* Next Button */
                $instance.find(".slideNext").on("click", function (e) {
                    
                    e.stopPropagation();

                    content.slideNext();
                });

                /* Previous Button */
                $instance.find(".slidePrev").on("click", function (e) {
                    
                    e.stopPropagation();
                    
                    content.slidePrev();
                });

                $instance.find(".bullets .bullet").on("click", function (e) {
                    
                    e.stopPropagation();
                    
                    if ($instance.find(".slide").is(":animated")) {
                        return;
                    }
                    
                    if ($(this).hasClass("active")) {
                        return;
                    }

                    glob.options.cs = $(this).index();

                    $instance.find(".timers .timer .progress").stop();
                    
                    var $reverse = $instance.find(".timers .timer:gt(" + glob.options.cs + ") .progress, .timers .timer:eq(" + glob.options.cs + ") .progress").get().reverse();
                    var dgt = glob.options.duration / $($reverse).length;
                    
                    $($reverse).each(function (i) {
                        var th = this;
                        setTimeout(function () {
                            $(th).animate({
                                width: 0
                            }, {
                                queue: false,
                                duration: dgt,
                                easing: "linear"
                            });
                        }, i * dgt);
                        
                    });
                    
                    var dlt = glob.options.duration / $instance.find(".timers .timer:lt(" + glob.options.cs + ")").length;
                    $instance.find(".timers .timer:lt(" + glob.options.cs + ") .progress").each(function (i) {
                        var th = this;
                        setTimeout(function () {
                            $(th).animate({
                                width: "100%"
                            }, {
                                queue: false,
                                duration: dlt,
                                easing: "linear"
                            });
                        }, i * dlt);
                    });


                    content.show();
                });
            }
            
        };
        
        var auto = {
            /* Run timer */
            run: function () {

                if (glob.options.pause === 0) {
                    return;
                }

                $instance.find(".timer:eq(" + glob.options.cs + ") .progress").animate({
                    width: "100%"
                }, {
                    queue: false,
                    duration: (glob.options.pause - (glob.options.pause / 100) * ((($instance.find(".timer:eq(" + glob.options.cs + ") .progress").width() / $instance.find(".timer:eq(" + glob.options.cs + ")").width()) * 100))),
                    easing: "linear",
                    complete: function () {
                        content.slideNext();
                    }
                });
            },
            /* Stop on focus */
            focus: function () {

                if (glob.options.hoverpause === 1) {
                    $instance.find(".slide_content").on("mouseover", function () {
                        if ($instance.find(".slide").is(":animated"))
                            return;
                        $instance.find(".timer .progress").stop();
                    });

                    $instance.find(".slide_content").on("mouseleave", function () {
                        if ($instance.find(".slide").is(":animated"))
                            return;
                        auto.run();
                    });
                }
            }
        };
        
        this.start = function () {
            if (typeof glob.options.beforeInit === "function") {
                glob.options.beforeInit();
            }
            content.init();
            controls.bindControls();
            return this;
        };
        
        this.addSlide = function(slide) {
            glob.slides.push(slide); 
            return this;
        };
        
        this.settings = function(settings) {
            $.extend(glob.options, settings);
            return this;
        };
        
        this.destroy = function() {
            $instance
                    .stop()
                    .off()
                    .find("*")
                    .stop()
                    .off();
            $instance.remove();
            
            return this;
        };
        
        this.restart = function() {
            
            glob.options.cs = 0;
            
            $instance
                    .stop()
                    .off()
                    .find("*")
                    .stop()
                    .off();
            $instance.empty();
            
            this.start();
            return this;
        };
        
        this.getSlidesNum = function(){
            return glob.slides.length;
        };
        
        return this;
        
    };

})(jQuery);

