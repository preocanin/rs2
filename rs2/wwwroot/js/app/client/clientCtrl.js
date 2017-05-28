(function(){
    
    angular
        .module('app')
        .controller('clientCtrl', clientCtrl);
    
    clientCtrl.$inject = ['$routeParams', 'UserService'];
    function clientCtrl($routeParams, UserService) {
        "use strict";

        var vm = this;
        vm.username = '';
        
        UserService.GetById($routeParams.id)
            .then(function(response){
                vm.username = response.username;
            });
    
    }
    
})();