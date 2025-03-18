jQuery(document).ready(function($) {	

// $(".view-content .newsticker-jcarousellite").jCarouselLite({
// 		vertical: true,
// 		hoverPause:true,
// 		visible: 3,
// 		auto:3000,
// 		speed:10000
// 	});
// $(".slidermenubox").jCarouselLite({
// 		vertical: true,
// 		hoverPause:true,
// 		visible: 7,
// 		auto:800,
// 		speed:3500
// 	});

$('.slidermenubox .menubox-menu').slick({
    dots: false,
    vertical: true,
    arrows: false,
    infinite: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 0,
    speed: 700,
    cssEase: 'linear',
  });

  $('.archive-slider').slick({
    speed: 5000,
    autoplay: true,
    autoplaySpeed: 0,
    centerMode: true,
    cssEase: 'linear',
    slidesToShow: 1,
    slidesToScroll: 1,
    variableWidth: true,
    infinite: true,
    initialSlide: 1,
    arrows: false,
    buttons: false,
    pauseOnHover: false,
  });
  $('.archive-slider').hover(
    function() {
      $(this).slick('slickPause');
    }, 
    function() {
      $(this).slick('slickPlay');
    }
  );

  $('.hero-section-slider').slick({
    dots: true,
    arrows: true,
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    autoplay: true,
    fade: true,
    speed: 2000,
  });

});


