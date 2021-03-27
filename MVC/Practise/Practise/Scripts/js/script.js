/*=============================================================
                 Mobile Menu
===============================================================*/
$(function () {

    //Show mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });

    //Hide mobile nav
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });
});

/*FAQ accrodion*/

var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var panel = this.nextElementSibling;
    if (panel.style.maxHeight) {
      panel.style.maxHeight = null;
    } else {
      panel.style.maxHeight = panel.scrollHeight + "px";
    } 
  });
}

/*Show hide password*/
$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $(".show-pass");
  if (input.attr("type") === "password") {
    $(".show-pass").attr("type", "text");
  } else {
    $(".show-pass").attr("type", "password");
  }
});

/*For Selling Price*/
$(function () {
    
    $("input[name='IsPaid']").click(function () {
        
        if ($("#paid").is(":checked")) {

            $("#selling").removeAttr("disabled");

            $("#selling").focus();
        } else {
            if ($("#free").is(":checked")) {
                $("#selling").val(0);
            }
        }
    });
});






















