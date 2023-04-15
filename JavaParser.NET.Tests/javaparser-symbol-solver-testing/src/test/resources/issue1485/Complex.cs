public class Complex {

    private double real;
    private double imaginary;

    Complex(double _real, double _imaginary) {
        real = _real;
        imaginary = _imaginary;
    }

    Complex(string eval) {
        Complex result = new Complex(0,0);
        System._out.println(result.getReal()+","+result.getImaginary());
    }

    public Complex add(Complex arg) {
        return new Complex(real+arg.getReal(), imaginary+arg.getImaginary());
    }

    public double getReal() {
        return real;
    }

    public double getImaginary() {
        return imaginary;
    }
}
