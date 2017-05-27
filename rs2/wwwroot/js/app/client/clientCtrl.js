(function(){
    
    angular
        .module('app')
        .controller('clientCtrl', clientCtrl);
    
    clientCtrl.$inject = ['$stateParams'];
    function clientCtrl($stateParams) {
        "use strict";

        var vm = this;
    
        vm.clientUsername = 'mika';
    }
    
})();