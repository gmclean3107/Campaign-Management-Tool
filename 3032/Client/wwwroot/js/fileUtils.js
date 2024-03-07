window.saveAsFile = function (fileName, byteBase64) {
    const linkSource = `data:application/octet-stream;base64,${byteBase64}`;
    const downloadLink = document.createElement("a");
    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
};
