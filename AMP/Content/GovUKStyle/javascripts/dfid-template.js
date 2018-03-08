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


// Simple Accordion script - used on Tabs demo
$(document).ready(function($) {
  $(".accordion-t-content").slideUp('fast');
  $('#accordion-t').find('.accordion-t-toggle').click(function(){

    //Expand or collapse this panel
    $(this).next().slideToggle('medium');
    $(this).toggleClass('visible');
    $(".accordion-t-toggle").not(this).removeClass('visible');

    //Hide the other panels
    $(".accordion-t-content").not($(this).next()).slideUp('medium');

  });
});


// Simple Accordion script 1 - copied to allow multi-accordions on page
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
// Simple Accordion script 2 - copied to allow multi-accordions on page
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
// Simple Accordion script 3 - copied to allow multi-accordions on page
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
// Simple Accordion script 4 - copied to allow multi-accordions on page
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

// Drop menu
$(document).ready( function() {
  $('.primary-menu > li').bind('mouseover', openSubMenu);
  $('.primary-menu > li').bind('mouseout', closeSubMenu);
  function openSubMenu() {
    $(this).find('ul').css('visibility', 'visible');
  }
  function closeSubMenu() {
    $(this).find('ul').css('visibility', 'hidden');
  }
});

// Simple function to display hidden rows in table
$(function() {
  $('tr.parent td')
    .on("click","span.btn", function(){
      var idOfParent = $(this).parents('tr').attr('id');
      $('tr.child-'+idOfParent).toggle('slow');
    });
});

// Function to allow adding/editing/saving/deleting table rows
$(function(){

  function Add_row(){
    $("#tblOutputDesc tbody").append(
      "<tr>"+
      "<td><input type='text'/></td>"+
      "<td><input type='text'/></td>"+
      "<td><input type='text'/></td>"+
      "<td><input type='text'/></td>"+
      "<td><img src='images/disk.png' class='btnSave_row'><img src='images/delete.png' class='btnDelete_row'/></td>"+
      "</tr>");

    $(".btnSave_row").bind("click", Save_row);
    $(".btnDelete_row").bind("click", Delete_row);
  };

  function Edit_row(){
    var wrap = $(this).parent().parent(); //tr
    var td_Out_Desc = wrap.children("td:nth-child(1)");
    var td_Imp_Wgt = wrap.children("td:nth-child(2)");
    var td_Out_Per = wrap.children("td:nth-child(3)");
    var td_Risk = wrap.children("td:nth-child(4)");
    var td_Ctrl = wrap.children("td:nth-child(5)");

    td_Out_Desc.html("<input type='text' id='Out_Desc' value='"+td_Out_Desc.html()+"'/>");
    td_Imp_Wgt.html("<input type='text' id='Imp_Wgt' value='"+td_Imp_Wgt.html()+"'/>");
    td_Out_Per.html("<input type='text' id='Out_Per' value='"+td_Out_Per.html()+"'/>");
    td_Risk.html("<input type='text' id='Risk' value='"+td_Risk.html()+"'/>");
    td_Ctrl.html("<img src='images/disk.png' class='btnSave_row'/>");

    $(".btnSave_row").bind("click", Save_row);
    $(".btnEdit_row").bind("click", Edit_row);
    $(".btnDelete_row").bind("click", Delete_row);
  };

  function Save_row(){
    var wrap = $(this).parent().parent(); //tr
    var td_Out_Desc = wrap.children("td:nth-child(1)");
    var td_Imp_Wgt = wrap.children("td:nth-child(2)");
    var td_Out_Per = wrap.children("td:nth-child(3)");
    var td_Risk = wrap.children("td:nth-child(4)");
    var td_Ctrl = wrap.children("td:nth-child(5)");

    td_Out_Desc.html(td_Out_Desc.children("input[type=text]").val());
    td_Imp_Wgt.html(td_Imp_Wgt.children("input[type=text]").val());
    td_Out_Per.html(td_Out_Per.children("input[type=text]").val());
    td_Risk.html(td_Risk.children("input[type=text]").val());
    td_Ctrl.html("<img src='images/delete.png' class='btnDelete_row'/><img src='images/pencil.png' class='btnEdit_row'/>");

    $(".btnEdit_row").bind("click", Edit_row);
    $(".btnDelete_row").bind("click", Delete_row);
  };

  function Delete_row(){
    var wrap = $(this).parent().parent(); //tr
    wrap.remove();
  };

  $(".btnEdit_row").bind("click", Edit_row);
  $(".btnDelete_row").bind("click", Delete_row);
  $("#btnAdd_row").bind("click", Add_row);

});


    //Menu items 
    $(document).ready(function () { 
        $('.primary-menu > li').bind('mouseover', openSubMenu); 
        $('.primary-menu > li').bind('mouseout', closeSubMenu); 
 
        function openSubMenu() { 
            $(this).find('ul').css('visibility', 'visible'); 
        } 
 
        function closeSubMenu() { 
            $(this).find('ul').css('visibility', 'hidden'); 
        } 
    });



// Tabs function
$(document).ready( function() {
  $('#tab-container').easytabs();
  $('#tab-container2').easytabs();
  $('#tab-container3').easytabs();
  $('#tab-container4').easytabs();
});

// Checkboxes
$(function() {
  $('label.block-label').click(function() {
    if ($(this.children).is(":checked"))
    {
      $(this).toggleClass('selected');
    }
  });
});


