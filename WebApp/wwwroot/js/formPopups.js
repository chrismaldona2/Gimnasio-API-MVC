

const signUpBtn = document.querySelector('.admin-signup-btn');
const popUpForm = document.querySelector('.signup-admin-form');
const popUpFormCancelBtn = document.querySelector('.signup-admin-form .cancel-btn');

signUpBtn.addEventListener('click', () => {
    popUpForm.showModal()
})

popUpFormCancelBtn.addEventListener('click', () => {
    popUpForm.close()
})
