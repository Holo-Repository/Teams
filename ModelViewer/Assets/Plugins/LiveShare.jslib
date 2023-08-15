mergeInto(LibraryManager.library, {

  SyncRotation: function (x,y,z) {
    window.syncCurrentRotation(x,y,z);
  },

  SyncScale: function (x,y,z) {
    window.syncCurrentScale(x,y,z);
  },

  SyncTexture: function (name, texture) {
    window.syncCurrentTexture(Pointer_stringify(name), Pointer_stringify(texture));
  },

  HandleScreenshotDataURL: function(dataURL) {
    window.handleScreenshotDataURL(dataURL);
  },

  JSConsoleLog: function (str) {
    console.log(UTF8ToString(str));
  }

});