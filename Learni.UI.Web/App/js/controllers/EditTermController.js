'use strict';

learniApp.controller('EditTermController',
    function EditTermController($scope, $modalInstance, $upload, termsService, packageObject, term) {
        $scope.uploading = false;
        $scope.uploaded = false;

        if (term) {
            $scope.inEditMode = true;
            $scope.term = term;

            if(term.imagePath)
                $scope.uploaded = true;
        } else {
            $scope.inEditMode = false;
            $scope.term = {};
            
        }

        

        $scope.saveTerm = function (term, termForm) {
            if (termForm.$valid) {
                term.packageId = packageObject.id;
                termsService.saveTerm(term).then(function (savedTerm) {
                    $modalInstance.close(savedTerm);
                }, function () {
                    console.log("There was an error saving.");
                });
            }
        };

        $scope.cancelEdit = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.onFileSelect = function ($files) {
            if ($files.length > 0) {
                $scope.uploading = true;
                $scope.uploaded = false;
                $scope.progress = 0;

                var file = $files[0];
                $scope.upload = $upload.upload({
                    url: 'http://localhost:59643/api/termsimages',
                    method: 'POST',
                    file: file,
                }).progress(function (evt) {
                    $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
                }).success(function (data, status, headers, config) {
                    $scope.term.imagePath = data.replace(/['"]/g, '');
                    $scope.uploading = false;
                    $scope.uploaded = true;
                });
                //.error(...)
            }
        };

        $scope.cancelUpload = function () {
            $scope.upload.abort();
            $scope.upload = null;
            $scope.uploading = false;
            $scope.uploaded = false;
        };
    }
);