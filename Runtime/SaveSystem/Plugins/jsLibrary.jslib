mergeInto(LibraryManager.library, {
    loadData: function(yourkey){
        var returnStr = localStorage.getItem(UTF8ToString(yourkey)) || "";
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    saveData: function(yourkey, yourdata){
        localStorage.setItem(UTF8ToString(yourkey), UTF8ToString(yourdata));
    },
    deleteKey: function(yourkey){
        localStorage.removeItem(UTF8ToString(yourkey));
    },
    deleteAllKeys: function(prefix){
        var prefixStr = UTF8ToString(prefix);
        for (var i = localStorage.length - 1; i >= 0; i--) {
            var key = localStorage.key(i);
            if (key != null && key.startsWith(prefixStr)) {
                localStorage.removeItem(key);
            }
        }
    }
});
