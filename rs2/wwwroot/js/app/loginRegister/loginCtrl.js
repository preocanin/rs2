(function () {
    'use strict';

    angular
        .module('app')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$location', 'AuthenticationService', 'FlashService', '$rootScope', 'toastr'];
    function LoginController($location, AuthenticationService, FlashService, $rootScope, toastr) {
        var vm = this;

        vm.login = login;
        vm.logout = logout;

        vm.regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;

        (function initController() {
            // reset login status
            AuthenticationService.ClearCredentials();
        })();

        function login() {
            vm.dataLoading = true;
            AuthenticationService.Login(vm.email, vm.password, function (response) {
                if (response.status !== 200) {
                    toastr.error('Nepostojeci korisnik');
                    AuthenticationService.ClearCredentials();
                    vm.dataLoading = false;
                    vm.email = '';
                    vm.password = '';
                    return;
                }
                $rootScope.userId = response.data.userId;
                if (!response.error) {
                    AuthenticationService.SetCredentials(vm.email, vm.password);
                    if (response.data && response.data.role == 0) {
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