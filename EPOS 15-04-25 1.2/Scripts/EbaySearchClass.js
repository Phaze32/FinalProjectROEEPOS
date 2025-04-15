function _cb_findItemsByKeywords(root) {
    var items = root.findItemsByKeywordsResponse[0].searchResult[0].item || [];
    var html = [];
    html.push('<table width="100%" border="0" cellspacing="0" cellpadding="3"><tbody>');

    for (var i = 0; i < items.length; ++i) {
        var item = items[i];
        var title = item.title;
        var pic = item.galleryURL;
        var viewitem = item.viewItemURL;

        if (null != title && null != viewitem) {
            html.push('<tr><td>' + '<img src="' + pic + '" border="0">' + '</td>' +
              '<td><a href="' + viewitem + '" target="_blank">' + title + '</a></td></tr>');
        }
    }
    html.push('</tbody></table>');
    document.getElementById("results").innerHTML = html.join("");
}