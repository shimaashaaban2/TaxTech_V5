
function extractLangCookie() {
    const langCookie = document.cookie.split('; ').filter(i => i.includes('lang'));

    if(langCookie.length > 0)
        return langCookie[0].split('=')[1];
    return 'en';
}