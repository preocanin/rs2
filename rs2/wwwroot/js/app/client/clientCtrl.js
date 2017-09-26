(function () {

    angular
        .module('app')
        .controller('clientCtrl', clientCtrl);

    clientCtrl.$inject = ['$routeParams', 'UserService', 'AuthenticationService'];
    function clientCtrl($routeParams, UserService, AuthenticationService) {
        "use strict";

        var vm = this;
        vm.username = '';
        vm.changePassword = false;
        vm.regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;

        UserService.GetById($routeParams.id)
            .then(function (response) {
                vm.username = response.username;
            });

        vm.changePasswordButton = function () {
            vm.changePassword = !vm.changePassword;
        }

        vm.changePass = function () {
            AuthenticationService.ChangePass($routeParams.id, { 'NewPassword': vm.newPassword }, function (response) {
                if (response.status !== 200) {
                    //TODO napraviti odgovor za pogresnu sifru, validaciju
                    vm.email = '';
                    vm.newPassword = '';
                    return;
                }

                toastr.success('Uspesna promena lozinke');
                vm.email = '';
                vm.newPassword = '';
                vm.changePasswordButton();

            });
        }

    }

})();