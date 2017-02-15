;(function() {
  'use strict';

  angular
    .module('fullPage.js', [])
    .directive('fullPage', fullPage);

  fullPage.$inject = ['$timeout'];

  function fullPage($timeout) {
    var directive = {
      restrict: 'A',
      scope: {
        options: '=',
        controls: '=',
          allowScrolling: '='
      },
      link: link
    };

    return directive;

    function link(scope, element) {
      var pageIndex;
      var slideIndex;
      var afterRender;
      var onLeave;
      var onSlideLeave;

      scope.controls = {
        slideUp: slideUp,
          slideDown: slideDown,
          slideRight: slideRight,
          slideLeft: slideLeft
      }

      if (typeof scope.options === 'object') {
        if (scope.options.afterRender) {
          afterRender = scope.options.afterRender;
        }

        if (scope.options.onLeave) {
          onLeave = scope.options.onLeave;
        }

        if (scope.options.onSlideLeave) {
          onSlideLeave = scope.options.onSlideLeave;
        }
      } else if(typeof options === 'undefined') {
        scope.options = {};
      }

      function rebuild() {
        console.log("REBUILD");
        //IMPORTANT: USE $TIMEOUT not settimeout
        //having a $timeout here fixed using fullpage.js on multiple views. using timeout make angular execute this statement at the end of its digest cycle
        $timeout(function(){
            destroyFullPage();

            $(element).fullpage(sanatizeOptions(scope.options));

            if (typeof afterRender === 'function') {
                afterRender();
            }

            if (!scope.allowScrolling){
                disableScrolling();
            }
        }, 0);

      };

      var destroyFullPage = function() {
        if ($.fn.fullpage.destroy) {
          $.fn.fullpage.destroy('all');
        }
      };

      var sanatizeOptions = function(options) {
        options.afterRender = afterAngularRender;
        options.onLeave = onAngularLeave;
        options.onSlideLeave = onAngularSlideLeave;

        function afterAngularRender() {
          //We want to remove the HREF targets for navigation because they use hashbang
          //They still work without the hash though, so its all good.
          if (options && options.navigation) {
            $('#fp-nav').find('a').removeAttr('href');
          }

          if (pageIndex) {
            $timeout(function() {
              $.fn.fullpage.silentMoveTo(pageIndex, slideIndex);
            });
          }
        }

        function onAngularLeave(page, next){
          pageIndex = next;

          if (typeof onLeave === 'function') {
            onLeave();
          }
        }

        function onAngularSlideLeave(anchorLink, page, slide, direction, next) {
          pageIndex   = page;
          slideIndex  = next;

          if (typeof onSlideLeave === 'function') {
            onSlideLeave();
          }
        }



        return options;
      };

      element.on('$destroy', destroyFullPage);

      function slideUp(){
        $.fn.fullpage.moveSectionUp();
      }

      function slideDown(){
        $.fn.fullpage.moveSectionDown();
      }

      function slideRight(){
        $.fn.fullpage.moveSlideRight();
      }

      function slideLeft() {
        $.fn.fullpage.moveSlideLeft();
      }

      function disableScrolling(){
        $.fn.fullpage.setMouseWheelScrolling(false);
        $.fn.fullpage.setAllowScrolling(false);
      }

        //if we are using a ui-router, we need to be able to handle anchor clicks without 'href="#thing"'
        $(document).on('click', '[data-menuanchor]', function () {
            $.fn.fullpage.moveTo($(this).attr('data-menuanchor'));
        });

      rebuild();

    }
  }

})();
