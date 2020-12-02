export class errorObject{
    id: number;
    isValid: boolean;
    message: string;
    

    constructor(id: number, isValid: boolean, message: string){
        this.id = id;
        this.isValid = isValid;
        this.message = message;
    }
}