(function(){
    
    angular
        .module('app')
        .controller('clientsCtrl', clientsCtrl);
    
    clientsCtrl.$inject = ['UserService', '$location', '$rootScope', 'FlashService'];
    function clientsCtrl(UserService, $location, $rootScope, FlashService) {
        "use strict";

        var vm = this;
        vm.data = {};  //objekat ima counts i users
        
        UserService.GetAll()
                .then(function(response) {
                    vm.data = response;
                });
    }
    
})();
