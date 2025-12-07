(function () {
    document.querySelector('a').addEventListener('click', function (event) {
        [...$.app.nav.navItem].forEach((e, i) => {
            if (!e.className.includes('config-has-sub') && e.innerText.includes(event.innerText)) {
                [...$.app.nav.navItem].forEach((e, i) => {
                    e.classList.remove('active');
                });
                localStorage.setItem('config-current-nav-item', i);
                e.classList.add('active');
                localStorage.removeItem('config-has-sub');
            }
            else {
                e.onclick = function () {
                    localStorage.setItem('config-has-sub', i);
                }
            }
        });
    });
})();