window.addEventListener('load', function () {
    let isMenuCollapsed = localStorage.getItem('is_menu_collapsed');
    const menuToggle = document.querySelector('.nav-link.modern-nav-toggle.pr-0');

    if (isMenuCollapsed) {
        if (isMenuCollapsed == 0 && ![...document.body.classList].includes('menu-collapsed')) {
            menuToggle.click();
            localStorage.removeItem('has-sub');
        }
        else if (isMenuCollapsed == 1 && ![...document.body.classList].includes('menu-expanded')){
            menuToggle.click();
        }
    }
    else {
        localStorage.setItem('is_menu_collapsed', 1);
    }
});

document.querySelector('.nav-link.modern-nav-toggle.pr-0').addEventListener('click', function (e) {
    if (![...document.body.classList].includes('menu-collapsed')) {
        localStorage.setItem('is_menu_collapsed', 0);
    }
    else {
        localStorage.setItem('is_menu_collapsed', 1);
    }
});