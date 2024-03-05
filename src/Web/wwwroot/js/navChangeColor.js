const defaultColor = "#476985";
let colorToset = "";

colorToset = window.localStorage.getItem('topNavColor') || defaultColor;

loadNavSelectedColor(colorToset);

$(function () {
    $("#color-select").spectrum({
        //flat: true,
        showPaletteOnly: true,
        togglePaletteOnly: true,
        togglePaletteMoreText: 'more',
        togglePaletteLessText: 'less',
        //flat: true,
        showInput: true,
        showInitial: true,
        palette: [
            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
            ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
        ],
        change: function (color) {
            loadNavSelectedColor(color);
        },
        move: function (color) {
            loadNavSelectedColor(color);
        }
    });

    $(".color-item").click(function () {
        let color = $(this).data("value");
        loadNavSelectedColor(color);
    });

    $("#color-select").spectrum("set", colorToset);

    $("#color-select").on("input change", function () {
        let color = $(this).val();
        loadNavSelectedColor(color);
    });

    $("#resetBtn").click(function () {
        loadNavSelectedColor(defaultColor);
    });
})

function loadNavSelectedColor(color = "") {
    let colorToSet = color == "" ? defaultColor : color;

    window.localStorage.setItem('topNavColor', colorToSet);
    let changeColorStyle = $("#changeColorStyle");

    let cssToInsert = `
    html[data-menu-color=light] {
        --ct-menu-bg: #ffffff;
        --ct-menu-item-color: #6e768e;
        --ct-menu-item-hover-color: #348cd4;
        --ct-menu-item-active-color: #348cd4;
    }

    html[data-bs-theme=dark][data-menu-color=light],
    html[data-menu-color=dark] {
        --ct-menu-bg: #38414a;
        --ct-menu-item-color: #8391a2;
        --ct-menu-item-hover-color: #bccee4;
        --ct-menu-item-active-color: #ffffff;
    }

    html[data-menu-color=brand] {
        --ct-menu-bg: ${colorToSet};
        --ct-menu-item-color: #cedce4;
        --ct-menu-item-hover-color: #ffffff;
        --ct-menu-item-active-color: #ffffff;
    }

    html[data-menu-color=gradient] {
        --ct-menu-bg: #1b286c;
        --ct-menu-item-color: #cedce4;
        --ct-menu-item-hover-color: #ffffff;
        --ct-menu-item-active-color: #ffffff;
        --ct-menu-item-active-bg: rgba(255, 255, 255, 0.2);
        --ct-menu-gradient-image: linear-gradient(270deg, rgba(64, 149, 216, 0.15), transparent);
    }

     html[data-topbar-color=light] {
        --ct-topbar-bg: #ffffff;
        --ct-topbar-item-color: #6e768e;
        --ct-topbar-item-hover-color: #348cd4;
        --ct-topbar-search-bg: #eef3f6;
    }

    html[data-bs-theme=dark][data-topbar-color=light],
    html[data-topbar-color=dark] {
        --ct-topbar-bg: #38414a;
        --ct-topbar-item-color: #8391a2;
        --ct-topbar-item-hover-color: #bccee4;
        --ct-topbar-search-bg: #464f5b;
    }

    html[data-topbar-color=brand] {
        --ct-topbar-bg: ${colorToSet};
        --ct-topbar-item-color: rgba(255, 255, 255, 0.7);
        --ct-topbar-item-hover-color: #ffffff;
        --ct-topbar-search-bg: rgba(255, 255, 255, 0.1);
    }
    `;

    if (changeColorStyle.length == 1)
        changeColorStyle.html(cssToInsert);
    else
        $('head').append(` <style type="text/css" id="changeColorStyle">${cssToInsert}</style>`);
}