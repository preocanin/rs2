(function () {
    'use strict';
 
    angular
        .module('app')
        .controller('LoginController', LoginController);
 
    LoginController.$inject = ['$location', 'AuthenticationService', 'FlashService', '$rootScope'];
    function LoginController($location, AuthenticationService, FlashService, $rootScope) {
        var vm = this;
 
        vm.login = login;
        vm.logout = logout;
 
        (function initController() {
            // reset login status
            AuthenticationService.ClearCredentials();
        })();
 
        function login() {
            vm.dataLoading = true;
            AuthenticationService.Login(vm.email, vm.password, function (response) {
                if (!response.error) {
                    AuthenticationService.SetCredentials(vm.email, vm.password);
                    if(vm.email === 'admin@gmail.com' && vm.password === 'admin'){
                        $rootScope.isAdmin = true;
                    }
                    $location.path('/');
                } else {
                    FlashService.Error(response.error);
                    vm.dataLoading = false;
                }
            });
        };
        
        function logout() {
            $rootScope.globals.currentUser = {};
        };
    }
 
})();