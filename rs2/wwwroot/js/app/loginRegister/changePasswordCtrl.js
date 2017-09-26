(function () {

    angular
        .module('app')
        .controller('changePasswordCtrl', changePasswordCtrl);

    changePasswordCtrl.$inject = ['AuthenticationService', '$location', 'toastr'];
    function changePasswordCtrl(AuthenticationService, $location, toastr) {
        "use strict";
        var vm = this;
        vm.regularExpression = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;

        vm.changePass = function () {
            AuthenticationService.ChangePass(-1, { 'OldPassword': vm.oldPassword, 'NewPassword': vm.newPassword }, function (response) {
                if(response.status !== 200) {
                    //TODO napraviti odgovor za pogresnu sifru, validaciju
                    vm.email = '';
                    vm.oldPassword = '';
                    vm.newPassword = '';
                    return;
                }

                toastr.success('Uspesna promena lozinke');
                $location.path('/');


            });
        }

    }

})();