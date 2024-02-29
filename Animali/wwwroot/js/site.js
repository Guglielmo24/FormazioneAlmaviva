// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//carica la lista di animali e il form all'apertura della pagina
$(document).ready(function () {
    loadList();
    loadForm();
});

//caricamento lista
function loadList() {
    $.ajax({
        type: 'GET',
        url: 'Home/_ListaPersone',
        success: function (data) {
            $("#_ListaPersone").html(data);
        }
    });
}

//carica il form
function loadForm() {
    $.ajax({
        type: 'GET',
        url: 'Home/_FormPersone',
        success: function (data) {

            $("#_FormPersone").append(data);
        }
    });
}


/*delete persona*/
function DeletePersona(ID) {
    $.ajax({
        type: 'POST',
        url: 'Home/DeletePersona',
        data: { id: ID },
        success: function (data) {
            loadList();
        }
    });
}

//show popup
function showPopup(id) {
        $.ajax({
            type: 'POST',
            url: 'Home/_PopupPersona',
            data: { id: id },
            success: function (data) {
                $("#_PopupPersona").html(data);
            }
        });
}

function closePopup() {
    document.getElementById('_PopupPersona').innerHTML = '';
}

function showAddPersonForm() {
    $("#addPersonPopup").show(); 
    $(".backdrop").show(); 
}

function closeAddPersonForm() {
    $("#addPersonPopup").hide();
    $(".backdrop").hide(); 
}
