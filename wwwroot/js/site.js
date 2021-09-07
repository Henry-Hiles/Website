window.addEventListener('DOMContentLoaded', () => {
    var navbarShrink = () => {
        const navbarCollapsible = document.body.querySelector('#header');
        if (!navbarCollapsible) {
            return;
        }
        if (window.scrollY === 0) {
            navbarCollapsible.classList.remove('navbar-shrink')
        } else {
            navbarCollapsible.classList.add('navbar-shrink')
        }
    };

    navbarShrink();
    document.addEventListener('scroll', navbarShrink);
});