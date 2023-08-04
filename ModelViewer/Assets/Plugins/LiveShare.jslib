mergeInto(LibraryManager.library, {

  SyncRotation: function (x,y,z) {
    window.parent.syncCurrentRotation(x,y,z);
  },

  HandleScreenshotDataURL: function(dataURL) {
    window.parent.handleScreenshotDataURL(dataURL);
  },

  JSConsoleLog: function (str) {
    console.log(UTF8ToString(str));
  }

});