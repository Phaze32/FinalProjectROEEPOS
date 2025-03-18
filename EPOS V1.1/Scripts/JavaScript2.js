var xmlHttp = null; // XMLHttpRequest object 
var ddlCountry = null; // DropDownList for country 
var ddlRegion = null; // DropDownList for region 
var ddlCity = null;  // DropDownList for city 
var hdfRegion = null; // Use to save region DropDownList options and restore it when page is rendering 
var hdfCity = null;  // Use to save city DropDownList options and restore it when page is loadrenderinged 
var hdfRegionSelectValue = null; // Use to save region DropDownList selected options and restore it when page is rendering 
var hdfCitySelectValue = null; // Use to save city DropDownList selected options and restore it when page is rendering 
var hdfCountrySelectValue = null;   // Use to save country DropDownList selected options and restore it when page is rendering 


// Instance XMLHttpRequest 
window.onload = function loadXMLHttp() {
    if (window.XMLHttpRequest) {
        xmlHttp = new XMLHttpRequest();
    } else if (window.ActiveXObject) {
        try {
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (e) { }
    }


    ddlCountry = document.getElementById('ddlCountry');
    ddlRegion = document.getElementById('ddlRegion');
    ddlCity = document.getElementById('ddlCity');
    hdfRegion = document.getElementById('hdfRegion');
    hdfCity = document.getElementById('hdfCity');
    hdfRegionSelectValue = document.getElementById('hdfRegionSelectValue');
    hdfCitySelectValue = document.getElementById('hdfCitySelectValue');
    hdfCountrySelectValue = document.getElementById('hdfCountrySelectValue');
    ShowFirstOption(ddlCountry); // Add "Please Select a xxxx" option in the top of country DropDownList  
    RestoreDropdownlist(); // Restore dropdownlist when page is rendering 


}


// Restore dropdownlist when page is rendering 
function RestoreDropdownlist() {
    // Restore region dropdownlist 
    if (hdfRegion.value != "") {
        addOption(ddlRegion, hdfRegion.value);
        ddlRegion.selectedIndex = hdfRegionSelectValue.value;
    } // Restore city dropdownlist 
    if (hdfCity.value != "") {
        addOption(ddlCity, hdfCity.value);
        ddlCity.selectedIndex = hdfCitySelectValue.value;
    }
    ddlCountry.selectedIndex = hdfCountrySelectValue.value;
}


// Save selected options in hide field so that we can access it from codebehind class 
function SaveSelectedData() {


    hdfCitySelectValue.value = ddlCity.selectedIndex;
    hdfCountrySelectValue.value = ddlCountry.selectedIndex;
    hdfRegionSelectValue.value = ddlRegion.selectedIndex;


    if (ddlCity != null && ddlCountry != null && ddlRegion != null
     && ddlCity.length > 0 && ddlCountry.length > 0 && ddlRegion.length > 0
     && ddlCity.selectedIndex != '0' && ddlCountry.selectedIndex != '0' && ddlRegion.selectedIndex != '0') {


        var strResult = ddlCountry.options[ddlCountry.selectedIndex].value + '; '
        + ddlRegion.options[ddlRegion.selectedIndex].value + '; '
        + ddlCity.options[ddlCity.selectedIndex].value;


        document.getElementById('hdfResult').value = strResult;




    }
    else {
        document.getElementById('hdfResult').value = 'Please select option from DropDownList.';
    }
}