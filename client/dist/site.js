var apiUrl = "http://localhost:7171/Produto.svc/";
var ajaxRequest = new XMLHttpRequest();

function isNullOrEmpty(value) {
    return typeof value === "undefined" || value == null || value == "" || (typeof value === "string" && value.trim() == "") || (Array.isArray(value) && value.length < 1);
}

function fillYear() {
    document.getElementById("year").innerHTML = new Date().getFullYear();
}

// https://stackoverflow.com/questions/8567114/how-to-make-an-ajax-call-without-jquery
function ajax(verb, url, data, callback) {
    ajaxRequest.abort();

    ajaxRequest.onreadystatechange = function() {
        if (ajaxRequest.readyState == XMLHttpRequest.DONE) 
            if(ajaxRequest.status == 0) // -> off-line
                document.getElementById("container").innerHTML = getOfflineContent(); 
            else
                callback(ajaxRequest.status, ajaxRequest.responseText);
    };

    ajaxRequest.open(verb, url, true);
    ajaxRequest.send(data);
}

document.addEventListener("DOMContentLoaded", function(){
    fillYear();
    getProducts(1);
});

function getProducts(page) {
    var value = document.getElementById("searchBar").value;
    var endpoint = "product/?page=";

    if (isNullOrEmpty(page))
        endpoint += 1;
    else
        endpoint += page;

    if (isNullOrEmpty(value))
        value = "";
    else
        value = "&value=" + encodeURIComponent(value);

    ajax("get", apiUrl + endpoint + value, null, buildProductsList);
}

function buildProductsList(status, responseText) {
    if(status == 200) {
        var template = `
            <div class="item-product">
                <a href="javascript:;" onclick="getProduct({id})">
                    <div class="imagem-product">
                        <span class="imagem-product-helper"></span>
                        <img src="{pictureUrl}">
                    </div>
                    <div>
                        <p>
                            {name}
                        </p>
                    </div>
                    <div class="price-product">
                        <h2>
                            R$ {price}
                        </h2>
                    </div>
                </a>
            </div>
        `;

        var producsPage = JSON.parse(responseText);
        var html = `<div class="flex-container">`;

        producsPage.Items.forEach(element => {
            html += template
                        .replace("{price}", element.Price)
                        .replace("{id}", element.Id)
                        .replace("{name}", element.Name)
                        .replace("{pictureUrl}", element.PictureUrl);
        });

        var paginatingHtml = buildPaginating(producsPage.Page, producsPage.TotalPages);
        html += `</div>` + paginatingHtml;

        document.getElementById("container").innerHTML = html;
    }
}

function buildPaginating(selectedPage, totalPages) {
    var template = `
        <a class="{style}" onclick="getProducts({page})" href="javascript:;">
            {page}
        </a>
    `;

    var html = "<div class='text-center space-div'>";

    for(var i = 1; i <= totalPages; i++) {
        var style = "pagination";

        if(selectedPage == i)
            style += " page-selected";

        html += template.replace(/\{page\}/g, i).replace(/\{style\}/g, style);
    }

    html += "</div>"

    return html;
}

function getProduct(id) {
    ajax("get", apiUrl + "product/" + encodeURIComponent(id) , null, buildProduct)
}

function buildProduct(status, responseText) {
    if(status == 200) {
        var template = `
            <div class="div-product">
                <h2>
                    {name}
                </h2>    
                <div class="div-image text-center">
                    <img src="{pictureUrl}">
                </div>
                <p>
                    {description}
                </p>
                <h2>
                    R$ {price}
                </h2>
                <p class="text-right">
                    <button onclick="getProducts(1)">Back</button>
                </p>
            </div>`;

        var element = JSON.parse(responseText);
        var html = template
                    .replace("{price}", element.Price)
                    .replace("{description}", element.Description)
                    .replace("{name}", element.Name)
                    .replace("{pictureUrl}", element.PictureUrl);
    
        document.getElementById("container").innerHTML = html;
    }
}

function getOfflineContent() {
    var html = `
        <div class="text-center">
            <h2>Aplicativo off-line!</h2>
            <p>
                <img class="img-80-max" src="off-line.png">
            </p>
            <p>
                <a href="index.html">Tente novamente</a> 
                quando estiver conectado.
            </p>
            <br>
        </div>
    `;

    return html;
}