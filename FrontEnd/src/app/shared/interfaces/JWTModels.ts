export  class JWT {
    PKUser: string;
    Role: string;
    NTUser: string;
    nbf?: number; // Optional property for "Not Before" timestamp
    exp?: number; // Optional property for expiration timestamp

    constructor(options: {
        PKUser: string,
        Role: string,
        NTUser: string,
        nbf?: number,
        exp?: number
    } = {PKUser:"",Role:"",NTUser:""}) {
        this.PKUser = options.PKUser;
        this.Role = options.Role;
        this.NTUser = options.NTUser;

        // Optional properties
        if (options.nbf) {
            this.nbf = options.nbf;
        }

        if (options.exp) {
            this.exp = options.exp;
        }
    }
}