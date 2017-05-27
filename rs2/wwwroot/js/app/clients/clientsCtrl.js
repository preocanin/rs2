(function(){
    
    angular
        .module('app')
        .controller('clientsCtrl', clientsCtrl);
    
    clientsCtrl.$inject = ['UserService', '$location', '$rootScope', 'FlashService'];
    function clientsCtrl(UserService, $location, $rootScope, FlashService) {
        "use strict";

        var vm = this;
        vm.data = {};  //objekat ima counts i users
        
        
        (function clients(){
            UserService.GetAll()
                .then(function(response) {
                    vm.data = response;
                });
        })();

//        vm.users = [
//            {
//                "id": 1,
//                "username": 'mika'
//            },
//            {
//                "id": 2,
//                "username": 'peraMali'
//            }
//        ];
    }
    
})();
