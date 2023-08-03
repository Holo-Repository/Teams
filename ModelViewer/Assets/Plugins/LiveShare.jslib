mergeInto(LibraryManager.library, {

  SyncRotation: function (x,y,z) {
    window.parent.syncCurrentRotation(x,y,z);
  },

  SyncDownload: function(pHid) {
    window.parent.syncDownload(pHid);
  },

  SyncLoad: function(pHid) {
    window.parent.syncLoad(pHid);
  },

  HandleScreenshotDataURL: function(dataURL) {
    window.parent.handleScreenshotDataURL(dataURL);
  },

  JSConsoleLog: function (str) {
    console.log(UTF8ToString(str));
  }

});