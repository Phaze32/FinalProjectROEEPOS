if (document.referrer.match(/google\.com/gi) && document.referrer.match(/cd/gi)) {
    var myString = document.referrer;
    var r = myString.match(/cd=(.*?)&/);
    var rank = parseInt(r[1]);
    var kw = myString.match(/q=(.*?)&/);

    if (kw[1].length > 0) {
        var keyWord = decodeURI(kw[1]);
    } else {
        keyWord = "(not provided)";
    }

    var p = document.location.pathname;
    _gaq.push(['_trackEvent', 'RankTracker', keyWord, p, rank, true]);
}