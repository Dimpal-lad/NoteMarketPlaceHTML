/*=============================================================
                 Navigation
===============================================================*/
/* Show & Hide White Navigation */
/*$(function () {
    //show/hide nav on page load
    showHideNav();
    $(window).scroll(function () {
        //show/hide nav on window's scroll
        showHideNav();

    });

    function showHideNav() {

        if ($(window).scrollTop() > 50) {

            //Show white nav
            $("nav").addClass("white-nav-top");

            //Show dark logo
            $(".navbar-brand img").attr("src", "images/pre-login/logo-dark.png");
        } else {
            //Hide white nav
            $("nav").removeClass("white-nav-top");

            //Show normal logo
            $(".navbar-brand img").attr("src", "images/pre-login/top-logo.png");

        }

    }
});*/

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




























