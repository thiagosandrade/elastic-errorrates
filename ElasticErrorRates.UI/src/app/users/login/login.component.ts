import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiUserService } from '../../_shared/api/user/api-user.service';
import { MessageNotifierService } from '../../_shared/messageNotifier/messageNotifier.service';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: ApiUserService,
        private messageNotifierService : MessageNotifierService) {}

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            return;
        }

        this.loading = true;
        this.authenticationService.login(this.f.username.value, this.f.password.value)
            .then(
                async () => {
                    this.messageNotifierService.messageNotify('success', 'User logged in!');
                    this.router.navigate([this.returnUrl]);
                },
                async (error : string) => {
                    this.messageNotifierService.messageNotify('error', error);
                    this.loading = false;
                });
    }
}