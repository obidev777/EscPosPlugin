class PrinterEscPos {

    constructor(api = "http://127.0.0.1:4050/") {
        this.data = [];
        this.apiPath = api;
    }

    getPrinters = () => {
        return $.ajax({
            type: "GET",
            url: this.apiPath + "printers",
            async: false,
            crossDomain: true,
            dataType: "json",
            format: "json",
            success: function (response) {
                if (response.status == "OK") {
                    console.log("success");
                } else {
                    console.log("error");
                }
            }
        });
    }
    commit = () => {
        this.data.push({ "Name": "PrintDocument", "Args": [] });
        return $.ajax({
            type: "POST",
            url: this.apiPath + "commit",
            async: false,
            data: JSON.stringify(this.data),
            crossDomain: true,
            dataType: "json",
            format: "json",
            success: function (response) {
                if (response.status == "OK") {
                    console.log("success");
                } else {
                    console.log("error");
                }
            }
        });
    }

    initialize = () => {
        this.data.push({ "Name": "InitializePrint", "Args": [] });
    }
    connect = (printerName) => {
        this.data.push({ "Name": "Init", "Args": [printerName] });
    }
    printTest = () => {
        this.data.push({ "Name": "TestPrinter", "Args": [] });
    }
    text = (text) => {
        this.data.push({ "Name": "Append", "Args": [text] });
    }
    textFont = (text, font = 0) => {
        this.data.push({ "Name": "Font", "Args": [text, font] });
    }
    textBold = (text) => {
        this.data.push({ "Name": "BoldMode", "Args": [text] });
    }
    textUnderline = (text) => {
        this.data.push({ "Name": "UnderlineMode", "Args": [text] });
    }
    expandMode = (type = 0) => {
        this.data.push({ "Name": "ExpandedMode", "Args": [type] });
    }
    condenseMode = (type = 0) => {
        this.data.push({ "Name": "CondensedMode", "Args": [type] });
    }
    separator = (text = "-") => {
        if (text == "") {
            this.data.push({ "Name": "Separator", "Args": [] });
        } else {
            this.data.push({ "Name": "Separator", "Args": [text] });
        }
    }
    doublew2 = () => {
        this.data.push({ "Name": "DoubleWidth2", "Args": [] });
    }
    doublew3 = () => {
        this.data.push({ "Name": "DoubleWidth3", "Args": [] });
    }
    normalw = () => {
        this.data.push({ "Name": "NormalWidth", "Args": [] });
    }
    alignR = () => {
        this.data.push({ "Name": "AlignRight", "Args": [] });
    }
    alignL = () => {
        this.data.push({ "Name": "AlignLeft", "Args": [] });
    }
    alignC = () => {
        this.data.push({ "Name": "AlignCenter", "Args": [] });
    }
    lineH = (height = 10) => {
        this.data.push({ "Name": "SetLineHeight", "Args": [height] });
    }
    line = () => {
        this.data.push({ "Name": "NewLine", "Args": [] });
    }
    lines = (count = 1) => {
        this.data.push({ "Name": "NewLines", "Args": [count] });
    }
    code128 = (text = "") => {
        this.data.push({ "Name": "Code128", "Args": [text] });
    }
    code39 = (text = "") => {
        this.data.push({ "Name": "Code38", "Args": [text] });
    }
    ean13 = (text = "") => {
        this.data.push({ "Name": "Ean13", "Args": [text] });
    }
    openDrawer = (text = "") => {
        this.data.push({ "Name": "OpenDrawer", "Args": [] });
    }
    image = (imageurl = "", w = 50, h = 50, scale = false, center = 0) => {
        this.data.push({ "Name": "Image", "Args": [imageurl, w, h, scale, center] });
    }
    qrcode = (text = "", w = 50, h = 50, dpi = 10, scale = false, center = 0) => {
        this.data.push({ "Name": "QrCode", "Args": [text, w, h, dpi, scale, center] });
    }
    cut = () => {
        this.data.push({ "Name": "FullPaperCut", "Args": [] });
    }

}