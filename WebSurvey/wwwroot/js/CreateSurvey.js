function OnPasswordNeeded() {
    var hidingBlock = document.getElementById('hidingBlock');
    if (document.getElementById('checkHidingBlock').checked) {
        hidingBlock.removeAttribute('hidden');
    }
    else {
        hidingBlock.setAttribute('hidden', 'hidden');
    }
}

function OnSelectAnswer(number) {
    var AnswerSelect = document.getElementById('Questions[' + number + '].Type');
    if (AnswerSelect == null) {
        AnswerSelect = document.getElementById('Questions_' + number + '__Type');
    }
    switch (AnswerSelect.options[AnswerSelect.selectedIndex].value) {
        case '0':  //TEXT
            //Remove RADIO and CHECK
            var destroyElement = document.getElementById('addOptionBtn-' + number);
            if (destroyElement != null) {
                destroyElement.remove();
            }
            var destroyOptions = document.getElementsByName('optionContainerOf' + number);
            if (destroyOptions.length > 0) {
                if (confirm('При переключении данного вопроса на текст, все опции будут стёрты. Подтвердите действие.')) {
                    while (destroyOptions.length > 0) {
                        destroyOptions[0].remove();
                    }
                }
            }
            break;
        case '1':
        case '2':
            if (document.getElementById('addOptionBtn-' + number) == null) {
                var div = document.createElement('div');
                div.innerHTML = document.getElementById('OptionAddBtnBlock').innerHTML.replace(/{questionNumber}/g, number);
                var insertAfter = document.getElementById(number);
                insertAfter.insertAdjacentElement('afterend', div);
            }
            break;
    }
}

function RenameAllQuestions(element, id) {
    var identificator = element.id;
    element.setAttribute('id', id);
    //H5 edititng
    var element = document.getElementById('questionHeader-' + identificator);
    element.innerHTML = 'Вопрос #' + id;
    element.setAttribute('id', 'questionHeader-' + id);
    //Name Input
    element = document.getElementById('Questions[' + identificator + '].Name');
    if (element != null) {
        element.setAttribute('id', 'Questions[' + id + '].Name');
        element.setAttribute('name', 'Questions[' + id + '].Name');
    }
    else {
        element = document.getElementById('Questions_' + identificator + '__Name');
        element.setAttribute('id', 'Questions[' + id + '].Name');
        element.setAttribute('name', 'Questions[' + id + '].Name');
    }
    //Type input
    element = document.getElementById('Questions[' + identificator + '].Type');
    if (element != null) {
        element.setAttribute('id', 'Questions[' + id + '].Type');
        element.setAttribute('name', 'Questions[' + id + '].Type');
    }
    else {
        element = document.getElementById('Questions_' + identificator + '__Type');
        element.setAttribute('id', 'Questions[' + id + '].Type');
        element.setAttribute('name', 'Questions[' + id + '].Type');
    }
    element.setAttribute('onchange', 'OnSelectAnswer(' + id + ')');
    //'Delete' button
    element = document.getElementById('btn-delete-' + identificator);
    element.setAttribute('id', 'btn-delete-' + id);
    element.setAttribute('onclick', 'DeleteAndCheckQuestion(' + id + ')');
    //Validation If exists
    element = document.getElementById('validation-' + identificator);
    if (element != null) {
        element.setAttribute('id', 'validation-' + id);
        element.setAttribute('data-valmsg-for', 'Questions[' + id + '].Text');
    }
    //'Add Option' button
    element = document.getElementById('addOptionBtn-' + identificator);
    if (element != null) {
        element.setAttribute('id', 'addOptionBtn-' + id);
        element.setAttribute('onclick', 'AddOption(' + id + ')');
    }
    //Options
    containers = document.getElementsByName('optionContainerOf' + identificator);
    elements = document.querySelectorAll('[id^="Questions[' + identificator + '].Options["]');
    if (elements.length == 0) {
        elements = document.querySelectorAll('[id^="Questions_' + identificator + '__Options_"]');
    }
    buttons = document.querySelectorAll('[id^="delete-question-' + identificator + '-option-"]');
    for (var i = 0; i < elements.length; i++) {
        elements[i].setAttribute('id', 'Questions[' + id + '].Options[' + i + '].Text');
        elements[i].setAttribute('name', 'Questions[' + id + '].Options[' + i + '].Text');
        buttons[i].setAttribute('id', 'delete-question-' + id + '-option-' + i);
        buttons[i].setAttribute('onclick', 'DeleteAndCheckOption(' + id + ',' + i + ')');
        containers[0].setAttribute('id', 'question-' + id + '-option-' + i);
        containers[0].setAttribute('name', 'optionContainerOf' + id);
    }
}

function RenameAllOptions(questionNumber) {
    containers = document.getElementsByName('optionContainerOf' + questionNumber);
    elements = document.querySelectorAll('[id^="Questions[' + questionNumber + '].Options["]');
    if (elements.length == 0) {
        elements = document.querySelectorAll('[id^="Questions_' + questionNumber + '__Options_"]');
    }
    buttons = document.querySelectorAll('[id^="delete-question-' + questionNumber + '-option-"]');
    validations = document.querySelectorAll('[id^="validation-' + questionNumber + '-"]');
    for (var i = 0; i < elements.length; i++) {
        elements[i].setAttribute('id', 'Questions[' + questionNumber + '].Options[' + i + '].Text');
        elements[i].setAttribute('name', 'Questions[' + questionNumber + '].Options[' + i + '].Text');
        buttons[i].setAttribute('id', 'delete-question-' + questionNumber + '-option-' + i);
        buttons[i].setAttribute('onclick', 'DeleteAndCheckOption(' + questionNumber + ',' + i + ')');
        containers[i].setAttribute('id', 'question-' + questionNumber + '-option-' + i);
        if (validations[i] != null) {
            validations[i].setAttribute('id', 'validation-' + questionNumber + '-' + i);
            validations[i].setAttribute('data-valmsg-for', 'Questions[' + questionNumber + '].Options[' + i + '].Text');
        }
    }
}

function DeleteAndCheckOption(questionNumber, optionNumber) {
    element = document.getElementById('question-' + questionNumber + '-option-' + optionNumber);
    $("input[id='Questions[" + questionNumber + "].Options[" + optionNumber + "].Text']").rules('remove', 'required');
    element.remove();
    RenameAllOptions(questionNumber);
}

function DeleteAndCheckQuestion(questionNumber) {
    var destroyOptions = document.getElementsByName('optionContainerOf' + questionNumber);
    var count = 0;
    while (destroyOptions.length > 0) {
        $("input[id='Questions[" + questionNumber + "].Options[" + count + "].Text']").rules('remove', 'required');
        count = count + 1;
        destroyOptions[0].remove();
    }
    var destroyElement = document.getElementById('addOptionBtn-' + questionNumber);
    if (destroyElement != null) {
        destroyElement.remove();
    }
    $("input[id='Questions[" + questionNumber + "].Name']").rules('remove', 'required');
    $('#' + questionNumber).remove();
    var elements = document.getElementsByName('questionContainer');
    for (var i = 0; i < elements.length; i++) {
        RenameAllQuestions(elements[i], i);
    }
}

function AddQuestion() {
    var questionCount = document.getElementsByName('questionContainer').length;
    var div = document.createElement('div');
    div.setAttribute('class', 'container mt-3');
    div.setAttribute('name', 'questionContainer');
    div.setAttribute('id', questionCount);
    var elementAfter = document.getElementById('addQuestionPoint');
    div.innerHTML = document.getElementById('QuestionBlock').innerHTML.replace(/{questionNumber}/g, questionCount);
    elementAfter.insertAdjacentElement('beforebegin', div);
    $("input[id='Questions[" + questionCount + "].Name']").rules('remove', 'required');
    div.scrollIntoView();
}

function AddOption(questionNumber) {
    var countOfOptions = document.getElementsByName('optionContainerOf' + questionNumber).length;
    var div = document.createElement('div');
    div.setAttribute('class', 'container');
    div.setAttribute('name', 'optionContainerOf' + questionNumber);
    div.setAttribute('id', 'question-' + questionNumber + '-option-' + countOfOptions);
    var elementAfter = document.getElementById('addOptionBtn-' + questionNumber);
    div.innerHTML = document.getElementById('OptionBlock').innerHTML.replace(/{questionNumber}/g, questionNumber).replace(/{optionNumber}/g, countOfOptions);
    elementAfter.insertAdjacentElement('beforebegin', div);
    $("input[id='Questions[" + questionNumber + "].Options[" + countOfOptions + "].Text']").rules('remove', 'required');
    div.scrollIntoView();
}