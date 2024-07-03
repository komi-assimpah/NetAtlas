//SIDEBAR
const menuItems = document.querySelectorAll('.menu-item');
//messages
const messagesNotifications = document.querySelector('#messages-notifications');
const messages = document.querySelector('.messages');
const message = messages.querySelectorAll('.message');
const messageSearch = document.querySelector('#message-search');
//theme
const theme = document.querySelector('#theme');
const themeModel = document.querySelector('.customize-theme');
const fontSizes = document.querySelectorAll('.choose-size span');
var root = document.querySelector(':root');
//for active
const removeSizeSelector = () => {
        fontSizes.forEach(size => {
            size.classList.remove('active');
        })
    }
    //for color
const colorPalette = document.querySelectorAll('.choose-color span');
//deplacer les actives
const changeActiveColorClass = () => {
        colorPalette.forEach(colorPicker => {
            colorPicker.classList.remove('active');
        })
    }
    //effacer les 'active'

const changeActiveItem = () => {
        menuItems.forEach(item => {
            item.classList.remove('active');
        })
    }
    //background
const Bg1 = document.querySelector('.bg-1');
const Bg2 = document.querySelector('.bg-2');
const Bg3 = document.querySelector('.bg-3');

//!----------------------------------------------------------------
menuItems.forEach(item => {
    item.addEventListener('click', () => {
        changeActiveItem();
        item.classList.add('active');
        if (item.id != 'notifications') {
            document.querySelector('.notifications-popup').style.display = 'none';
        } else {
            document.querySelector('.notifications-popup').style.display = 'block';
            document.querySelector('#notifications .notifications-count').style.display = 'none';
        }
    })
})

//messages
//search chats
const searchMessage = () => {
        const val = messageSearch.value.toLowerCase();
        message.forEach(chat => {
            let name = chat.querySelector('h5').textContent.toLowerCase();
            if (name.indexOf(val) != -1) {
                chat.style.display = 'flex';
            } else {
                chat.style.display = 'none';
            }
        })
    }
    //search chats
messageSearch.addEventListener('keyup', searchMessage);
//envoi à la partie message
messagesNotifications.addEventListener('click', () => {
    messages.style.boxShadow = '0 0 1rem var(--color-primary)';
    messagesNotifications.querySelector('.notifications-count').style.display = 'none';
    setTimeout(() => {
        messages.style.boxShadow = 'none';
    }, 2000);
})

//display theme
//open model
const openThemeModel = () => {
    themeModel.style.display = 'grid';
}

//closing model
const closeThemeModel = (e) => {
    if (e.target.classList.contains("customize-theme")) {
        themeModel.style.display = 'none';
    }
}

themeModel.addEventListener('click', closeThemeModel);
theme.addEventListener('click', openThemeModel);

//---------FONTS
fontSizes.forEach(size => {
    size.addEventListener('click', () => {
        removeSizeSelector();
        let fontSize;
        size.classList.toggle('active');

        if (size.classList.contains('font-size1')) {
            fontSize = '10px';
            root.style.setProperty('----sticky-top-left', '5.4rem');
            root.style.setProperty('----sticky-top-right', '5.4rem');
        } else if (size.classList.contains('font-size2')) {
            fontSize = '13px';
            root.style.setProperty('----sticky-top-left', '5.4rem');
            root.style.setProperty('----sticky-top-right', '-7rem');
        } else if (size.classList.contains('font-size3')) {
            fontSize = '16px';
            root.style.setProperty('----sticky-top-left', '-2rem');
            root.style.setProperty('----sticky-top-right', '-17rem');
        } else if (size.classList.contains('font-size4')) {
            fontSize = '19px';
            root.style.setProperty('----sticky-top-left', '-5rem');
            root.style.setProperty('----sticky-top-right', '-25rem');
        } else if (size.classList.contains('font-size5')) {
            fontSize = '22px';
            root.style.setProperty('----sticky-top-left', '-12rem');
            root.style.setProperty('----sticky-top-right', '-35rem');
        }
        document.querySelector('html').style.fontSize = fontSize;
    })
})

// change primary color
colorPalette.forEach(color => {
    color.addEventListener('click', () => {
        let primary;
        changeActiveColorClass();
        if (color.classList.contains('color-1')) {
            primaryHue = 252;
        } else if (color.classList.contains('color-2')) {
            primaryHue = 52;
        } else if (color.classList.contains('color-3')) {
            primaryHue = 352;
        } else if (color.classList.contains('color-4')) {
            primaryHue = 152;
        } else if (color.classList.contains('color-5')) {
            primaryHue = 202;
        }
        color.classList.add('active');
        root.style.setProperty('--primary-color-hue', primaryHue);
    })
})


//!BACKGROUND

let lightColorLightness;
let whiteColorLightness;
let darkColorLightness;

//changer le background
const changeBG = () => {
        root.style.setProperty('--light-color-lightness', lightColorLightness);
        root.style.setProperty('--white-color-lightness', whiteColorLightness);
        root.style.setProperty('--dark-color-lightness', darkColorLightness);
    }
    //if bg1
Bg1.addEventListener('click', () => {
    //add active class
    Bg1.classList.add('active');
    //remove active for other bg
    Bg2.classList.remove('active');
    Bg3.classList.remove('active');
    //remove customization
    window.location.reload();
})

//if bg2

Bg2.addEventListener('click', () => {
    darkColorLightness = '95%';
    whiteColorLightness = '20%';
    lightColorLightness = '15%';

    //add active class
    Bg2.classList.add('active');
    //remove active for other bg
    Bg1.classList.remove('active');
    Bg3.classList.remove('active');
    changeBG();
})

//if bg3

Bg3.addEventListener('click', () => {
    darkColorLightness = '95%';
    whiteColorLightness = '10%';
    lightColorLightness = '0%';

    //add active class
    Bg3.classList.add('active');
    //remove active for other bg
    Bg1.classList.remove('active');
    Bg2.classList.remove('active');
    changeBG();
})