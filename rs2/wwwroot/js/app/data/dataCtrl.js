(function () {

    angular
        .module('app')
        .controller('dataCtrl', dataCtrl);

    dataCtrl.$inject = ['RecordService', '$location', '$rootScope', 'toastr'];

    function dataCtrl(RecordService, $location, $rootScope, toastr) {
        "use strict";

        var vm = this;
        vm.unos = false;
        vm.prikaz = true;
        vm.podatak = {};
        vm.records = [];



        vm.kadUnos = function () {
            vm.unos = true;
            vm.prikaz = false;
        };

        vm.kadPrikaz = function () {
            vm.unos = false;
            vm.prikaz = true;

            RecordService.GetAll()
                .then(function (response) {
                    vm.records = response.records;
                });
        };

        vm.kadPrikaz();

        vm.dodaj = function () {
            if (validan(vm.podatak)) {
                RecordService.AddRecord(vm.podatak)
                    .then(function (response) {
                        console.log(response);
                        toastr.success('Podatak je dodat');
                    });
            }
            else {
                toastr.error('Niste popunili sva polja');
            }
        };

        vm.dodajExcel = function () {
            var fs = require('fs');
            var request = require('request'); // For sending requests
            //var tmp = require('tmp');
            //var XLSX = require('xlsx'); // Conversation berween formats .xlsx, .xls and other formats

            // Making temporary file usedi
            // Use if conversation on client side is supported
            /*
            var tmpPath = tmp.tmpNameSync({prefix: "tmp", postfix: ".xlsx"});
            var workbook = XLSX.readFile('./rezultati.xls');
            XLSX.writeFile(workbook, 'convert.xlsx');
            */

            // Sending a excel file to server
            var url = "http://localhost:5000/api/records/file";
            var form_data = { file: fs.createReadStream('./rezultati.xlsx') };
            var r = request.defaults({ jar: true });
            var jar = r.jar();
            
            // Use cookies only from Node.js 
            var cookie = r.cookie("access_token=token_string");
            jar.setCookie(cookie, url);
            
            r.post({ url: url, jar: jar, formData: form_data });
            

            // 
            /*
            var url = "http://localhost:5000/api/records/file";
            var r = request.defaults({ jar: true });
            var jar = r.jar();
            
            // Use cookies only from Node.js
            var cookie = r.cookie("access_token=token_string");
            jar.setCookie(cookie, url);
            
            r.get({ url: url, jar: jar }).pipe(fs.createWriteStream('./records.xlsx'))
            */
        };

        var validan = function (data) {

            if (data.Bx && data.By && data.Ax && data.Ay) {
                return true;
            }

            return false;
        };


        vm.obrisiSve = function() {
            RecordService.DeleteAllRecords()
                .then(function(response) {
                    console.log(response);
                });
        };

        vm.obrisiRed = function() {
            
        };

    }

})();