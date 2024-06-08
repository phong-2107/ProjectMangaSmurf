

/* ======================== active rank ================================ */
var header = document.getElementById("rank__type");
var btns = header.getElementsByClassName("nav__item");
for (var i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        var current = document.getElementsByClassName("active-rank");
        if (current.length > 0) {
            current[0].className = current[0].className.replace(" active-rank", "");
        }
        this.className += " active-rank";
    });
}

/*======================== FUNCTION UI ĐÁNH GIÁ ============================*/
const stars = document.querySelectorAll(".rating i");
stars.forEach((star, index1) => {
    star.addEventListener("click", () => {
        stars.forEach((star, index2) => {
            index1 >= index2 ? star.classList.add("active") : star.classList.remove("active");
        });
    });
});

/*======================== FUNCTION UI TINH ĐIEM ĐÁNH GIÁ ============================*/
const stars = document.querySelectorAll(".rating i");
stars.forEach((star, index1) => {
    star.addEventListener("click", () => {
        stars.forEach((star, index2) => {
            index1 >= index2 ? star.classList.add("active") : star.classList.remove("active");
        });
    });
});


/*=============== SHOW SCROLL UP ===============*/
const scrollUp = () => {
    const scrollUp = document.getElementById('scroll-up');
    this.scrollY >= 350 ? scrollUp.classList.add('show-scroll')
        : scrollUp.classList.remove('show-scroll')
}
window.addEventListener('scroll', scrollUp);


/*=============== SCROLL REVEAL ANIMATION ===============*/
const sr = ScrollReveal({
    origin: 'top',
    distance: '60px',
    duration: 2500,
    delay: 400,
})

/*=============== DARK LIGHT THEME ===============*/
const themeButton = document.getElementById('theme-button');
const darkTheme = 'dark-theme';
const iconTheme = 'ri-sun-cloudy-fill';

const selectedTheme = localStorage.getItem('selected-theme');
const selectedIcon = localStorage.getItem('selected-icon');

const getCurrentTheme = () => document.body.classList.contains(darkTheme) ? 'dark' : 'dark';
const getCurrentIcon = () => themeButton.classList.contains(iconTheme) ? 'ri-sun-cloudy-fill' : 'ri-moon-fill';

if (selectedTheme) {
    document.body.classList[selectedTheme === 'dark' ? 'add' : 'remove'](darkTheme)
    themeButton.classList[selectedIcon === 'ri-moon-fill' ? 'remove' : 'add'](iconTheme)
}
themeButton.addEventListener('click', () => {
    requestAnimationFrame(() => {
        document.body.classList.toggle(darkTheme);
        themeButton.classList.toggle(iconTheme);
    });

    localStorage.setItem('selected-theme', getCurrentTheme());
    localStorage.setItem('selected-icon', getCurrentIcon());
});



sr.reveal(`.new-update, .TruyenNew__Containter, .Update__Containter, .TopRank__container, .footer__description, .footer__content`)
sr.reveal(`.main__top` ,, { origin: 'bottom' })
sr.reveal(`.main__bottom, .new-update`, { origin: 'left' })
sr.reveal(`.about__img , .recently__img`, { origin: 'right' })
sr.reveal(`.popular__card`, { interval: 100 })


