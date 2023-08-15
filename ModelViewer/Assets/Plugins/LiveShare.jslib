mergeInto(LibraryManager.library, {

  SyncRotation: function (x,y,z) {
    window.syncCurrentRotation(x,y,z);
  },

  SyncScale: function (x,y,z) {
    window.syncCurrentScale(x,y,z);
  },

  SyncTexture: function (texture) {
    window.syncCurrentTexture(texture);
  },

  HandleScreenshotDataURL: function(dataURL) {
    window.handleScreenshotDataURL(dataURL);
  },

  JSConsoleLog: function (str) {
    console.log(UTF8ToString(str));
  }

});