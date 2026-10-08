const themeMeta = () => document.querySelector('meta[name="theme-color"]');

function applyTheme(theme, persist = true) {
    const normalized = theme === "dark" ? "dark" : "light";

    document.documentElement.dataset.theme = normalized;
    document.documentElement.style.colorScheme = normalized;

    const meta = themeMeta();
    if (meta) {
        meta.setAttribute("content", normalized === "dark" ? "#0a1020" : "#f4f7fb");
    }

    if (persist) {
        localStorage.setItem("examcalc-theme", normalized);
    }

    return normalized;
}

window.examCalc = {
    getTheme: function () {
        return document.documentElement.dataset.theme ||
            (window.matchMedia?.("(prefers-color-scheme: dark)").matches ? "dark" : "light");
    },

    setTheme: function (theme) {
        return applyTheme(theme, true);
    },

    toggleTheme: function () {
        const current = document.documentElement.dataset.theme ||
            (window.matchMedia?.("(prefers-color-scheme: dark)").matches ? "dark" : "light");

        return applyTheme(current === "dark" ? "light" : "dark", true);
    },

    copyText: async function (text) {
        if (navigator.clipboard && window.isSecureContext) {
            await navigator.clipboard.writeText(text);
            return;
        }

        const textArea = document.createElement("textarea");
        textArea.value = text;
        textArea.setAttribute("readonly", "");
        textArea.style.position = "fixed";
        textArea.style.opacity = "0";
        document.body.appendChild(textArea);
        textArea.select();

        try {
            document.execCommand("copy");
        } finally {
            textArea.remove();
        }
    }
};
