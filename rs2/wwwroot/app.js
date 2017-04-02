
'use strict';

/*global angular */
var app = angular.module('app', ['ui.router']);


app.config(['$urlRouterProvider', '$stateProvider', configRoutes])
    .run(['$rootScope', '$state', configureStateWithScope]);

//    app.controller('homeCtrl', homeCtrl);
//
//    function homeCtrl(){
//        
//    }

function configRoutes($urlRouterProvider, $stateProvider) {
    $urlRouterProvider.otherwise('/home');

    $stateProvider
        .state('home', {
            url: '/home',
            templateUrl: '~/templates/home.html',
            controllerAs: 'vm'
        })
        .state('login', {
            url: '/login',
            templateUrl: '~/templates/login.html',
            controller: 'loginRegisterCtrl',
            controllerAs: 'vm'
        })
        .state('register', {
            url: '/register',
            templateUrl: '~/templates/register.html',
            controller: 'loginRegisterCtrl',
            controllerAs: 'vm'
        })
        .state('data', {
            url: '/data',
            templateUrl: '~/templates/data.html',
            controller: 'dataCtrl',
            controllerAs: 'vm'
        })
        .state('clients', {
            url: '/clients',
            templateUrl: '~/templates/clients.html',
            controller: 'clientsCtrl',
            controllerAs: 'vm'
        })
        .state('clients/:id', {
            url: '/clients/:id',
            templateUrl: "~/templates/client.html",
            controller: 'clientCtrl',
            controllerAs: 'vm'
        });
}


function configureStateWithScope($rootScope, $state) {
    $rootScope.$state = $state;
}

