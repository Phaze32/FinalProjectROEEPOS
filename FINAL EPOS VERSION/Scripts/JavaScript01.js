function otherSelect() {
    var list = document.getElementById("<%=tripType.ClientID%>");
    var chosenItemText = list.value;
    if (chosenItemText == "Other") {
        document.getElementById('otherValueDiv').style.display = 'inline';
    }
    else {
        document.getElementById('otherValueDiv').style.display = 'none';
    }
}

function open_win(url_add)  
{   
    window.open(url_add,'welcome','width=500,height=300,menubar=no,status=no,location=no,resizable=1 ,toolbar=no,scrollbars=yes');  
}  
     
function ConfirmDelete()  
{  
 
    return confirm("Are you sure you wish to delete the selected item?");        
 
}  
 
