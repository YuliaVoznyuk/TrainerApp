window.JS = {
    Confirm: function (message) {
        return confirm(message);
    },
    Alert: function (message) {
        alert(message);
    },
    Prompt: function (message, defaultValue) {
        return prompt(message, defaultValue);
    }
};