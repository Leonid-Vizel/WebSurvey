function OnPasswordNeeded() {
    var hidingBlock = document.getElementById('hidingBlock');
    if (document.getElementById('checkHidingBlock').checked) {
        hidingBlock.removeAttribute('hidden');
    }
    else {
        hidingBlock.setAttribute('hidden', 'hidden');
    }
}

function AddOption() {
    var currentCount = document.getElementsByName('optionContainer').length;
    var div = document.createElement('div');
    div.setAttribute('class', 'container mt-3');
    div.setAttribute('name', 'optionContainer');
    div.setAttribute('id', currentCount);
    var elementAfter = document.getElementById('addOptionBtn');
    div.innerHTML = document.getElementById('OptionBlock').innerHTML.replace(/{Number}/g, currentCount);
    elementAfter.insertAdjacentElement('beforebegin', div);
    $("input[id='Options[" + currentCount + "].Text']").rules('remove', 'required');
    div.scrollIntoView();
}

function DeleteOption(number) {
    $("input[id='Options[" + number + "].Text']").rules('remove', 'required');
    document.getElementById(number).remove();
    RenameAllOptions();
}

function RenameAllOptions() {
    var options = document.getElementsByName('optionContainer');
    for (var i = 0; i < options.length; i++) {
        var ID = options[i].id;
        options[i].setAttribute('id', i);
        var button = document.getElementById('delete-' + ID);
        button.setAttribute('id', 'delete-' + i);
        button.setAttribute('onclick', 'DeleteOption(' + i + ')');
        var input = document.getElementById('Options[' + ID + '].Text');
        if (input == null) {
            input = document.getElementById('Options_' + ID + '__Text');
        }
        input.setAttribute('id', 'Options[' + i + '].Text');
        input.setAttribute('name', 'Options[' + i + '].Text');
        var validation = document.getElementById('validation-' + ID);
        if (validation != null) {
            validation.setAttribute('id', 'validation-' + i);
            validation.setAttribute('data-valmsg-for', 'Options[' + i + '].Text');
        }
    }
}