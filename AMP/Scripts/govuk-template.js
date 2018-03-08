//= require cookie-functions
//= require cookie-bar
//= req

// Simple Accordion script
$(document).ready(function($) {
  $(".accordion-content").slideUp('fast');
  $('#accordion').find('.accordion-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-content").not($(this).next()).slideUp('medium');

  });
});
// Simple Accordion script 1 - copied for quickness to allow multi-accordions on page
$(document).ready(function($) {
  $(".accordion-1-content").slideUp('fast');
  $('#accordion-1').find('.accordion-1-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-1-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-1-content").not($(this).next()).slideUp('slow');

  });
});
// Simple Accordion script 2 - copied for quickness to allow multi-accordions on page
$(document).ready(function($) {
  $(".accordion-2-content").slideUp('fast');
  $('.accordion-2').find('.accordion-2-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-2-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-2-content").not($(this).next()).slideUp('slow');

  });
});
// Simple Accordion script 3 - copied for quickness to allow multi-accordions on page
$(document).ready(function($) {
  $(".accordion-3-content").slideUp('fast');
  $('.accordion-3').find('.accordion-3-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-3-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-3-content").not($(this).next()).slideUp('slow');

  });
});
// Simple Accordion script 4 - copied for quickness to allow multi-accordions on page
$(document).ready(function($) {
  $(".accordion-4-content").slideUp('fast');
  $('.accordion-4').find('.accordion-3-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-4-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-4-content").not($(this).next()).slideUp('slow');

  });
});

// Simple Accordion script for Style Guide
$(document).ready(function($) {
  $(".accordion-sg-content").slideUp('fast');
  $('.accordion-sg').find('.accordion-sg-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-sg-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-sg-content").not($(this).next()).slideUp('slow');

  });
});

// Simple function to display hidden rows in table
$(function() {
  $('tr.parent td')
    .on("click","span.btn", function(){
      var idOfParent = $(this).parents('tr').attr('id');
      $('tr.child-'+idOfParent).toggle('slow');
    });
});